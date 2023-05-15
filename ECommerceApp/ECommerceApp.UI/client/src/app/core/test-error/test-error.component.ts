import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.scss']
})
export class TestErrorComponent {
  baseUrl = environment.apiUrl

  constructor(
    private http:HttpClient
  ){
    console.log(this.baseUrl+'product/42')
  }

  get404Error(){
    this.http.get(this.baseUrl + 'product/42').subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  get500Error(){
    this.http.get(this.baseUrl+ 'Buggy/servererror').subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  get400Error(){
    this.http.get(this.baseUrl + 'Buggy/badrequest').subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  get400ValidationError(){
    this.http.get(this.baseUrl + 'product/fortytwo').subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }
}
