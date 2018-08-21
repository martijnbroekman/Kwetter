import { Component, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'post-tweet',
  templateUrl: './post-tweet.component.html',
  styleUrls: ['./post-tweet.component.css']
})
export class PostTweetComponent implements OnInit {

  @Output('post') post = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

  onClick(value) {
    this.post.emit(value);
  }

}
