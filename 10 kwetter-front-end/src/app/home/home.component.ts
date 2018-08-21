import { Component, OnInit } from '@angular/core';
import { Kweet } from '../interfaces/Kweet';
import { KweetsService } from '../services/kweet.service';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  
  kweets: Kweet[] = [];

  constructor(
    private kweetService: KweetsService, 
    private authService: AuthService,
    private router: Router) { 
    this.kweets.push({
      id: 1,
      user: {
        userName: 'martijn123',
        firstName: '',
        middleName: '',
        lastName: '',
        id: 1,
        profileImageUrl: ''
      },
      description: 'My first tweet',
      date: new Date(2017, 1 ,1),
      isLiked: false,
      likesCount: 0
    });
  }

  ngOnInit() {
    if (this.authService.isLoggedIn())
      this.kweetService.getTimeLine(this.authService.currentUser.sub)
        .subscribe(kweets => this.kweets = kweets);
    else
      this.router.navigate(['/login'])
  }

  onTweet(value: string) {

    let kweet = {
      id: 1,
      user: {
        userName: 'martijn123',
        firstName: '',
        middleName: '',
        lastName: '',
        id: 1,
        profileImageUrl: ''
      },
      description: value,
      date: new Date(2017, 1 ,1),
      isLiked: false,
      likesCount: 0
    };
    
    this.kweets.splice(0, 0, kweet);
  }

}
