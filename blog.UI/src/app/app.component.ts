import { Component, OnInit } from '@angular/core';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';
import { ToastrService } from 'ngx-toastr';
import * as signalR from '@aspnet/signalr';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'app';
  constructor(private toastr: ToastrService) {}

  ngOnInit(): void {
    const connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl('https://localhost:44356/notification')
      .build();

    connection
      .start()
      .then(function() {
        console.log('Connected!');
      })
      .catch(function(err) {
        return console.error(err.toString());
      });

    connection.on(
      'BroadcastMessageAsync',
      (title: string, message: string, url: string, image: string) => {
        this.showToaster(message, title, url, image);
      }
    );
  }
  private showToaster(message: string, title: string, url: string, image) {
    var msg = '<img src="http://lorempixel.com/400/200/">';
    this.toastr
      .success(msg, title, {
        enableHtml: true,
        closeButton: true,
        timeOut: 10000
      })
      .onTap.pipe(take(1))
      .subscribe(() => this.toasterClickedHandler(url));
  }

  toasterClickedHandler(url) {
    console.log('Toastr clicked');
    window.location.href = url;
  }
}