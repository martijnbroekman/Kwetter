import { Component, OnInit, Input } from '@angular/core';
import { Kweet } from '../interfaces/Kweet';

@Component({
  selector: 'tweet',
  templateUrl: './tweet.component.html',
  styleUrls: ['./tweet.component.css']
})
export class TweetComponent implements OnInit {

  @Input('value') kweet: Kweet;

  constructor() { }

  ngOnInit() {
  }

}
