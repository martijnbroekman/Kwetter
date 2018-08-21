import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  private get isLoggedIn(): boolean { return this.authService.isLoggedIn() };
  private get userName() : string { return this.isLoggedIn ? this.authService.currentUser.name : ''; };

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit() {
  }

  signOff() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

}
