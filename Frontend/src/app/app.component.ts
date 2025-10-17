import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouteService, RouteResponse } from './services/route.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  nodes: string[] = [];
  startNode: string = '';
  endNode: string = '';
  result: RouteResponse | null = null;
  error: string = '';
  loading: boolean = false;

  constructor(private routeService: RouteService) {}

  ngOnInit() {
    this.loadNodes();
  }

  loadNodes() {
    this.routeService.getNodes().subscribe({
      next: (data) => {
        this.nodes = data;
      },
      error: (err) => {
        this.error = 'Failed to load nodes';
        console.error(err);
      }
    });
  }

  calculateRoute() {
    if (!this.startNode || !this.endNode) {
      this.error = 'Please select both start and end nodes';
      return;
    }

    this.loading = true;
    this.error = '';
    this.result = null;

    this.routeService.getShortestRoute(this.startNode, this.endNode).subscribe({
      next: (data) => {
        this.result = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = err.error?.message || 'Failed to calculate route';
        this.loading = false;
      }
    });
  }
}