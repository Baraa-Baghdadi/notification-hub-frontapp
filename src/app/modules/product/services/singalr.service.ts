import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr"
import { ToastrService } from 'ngx-toastr';
import { ProductService } from './product.service';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SingalrService {
  // Subject For save unreading messages:
  public unreadMsg = new BehaviorSubject<any[] | null>(null);
  unreadMsg$ = this.unreadMsg.asObservable();

  // UserName From Token:
  userName = "BARAABA";

  private hubConnection!: signalR.HubConnection;

  constructor(private toastr: ToastrService,public productService: ProductService , 
    private http : HttpClient) {
  }
    public startConnection = () => {
      this.hubConnection = new signalR.HubConnectionBuilder()
                              .withUrl('https://localhost:7132/Notify',{ skipNegotiation: true,
                              transport: signalR.HttpTransportType.WebSockets})
                              .build();
      this.hubConnection
        .start()
        .then((data:any) => console.log('Connection started'))
        .catch((err:any) => console.log('Error while starting connection: ' + err));

        this.hubConnection.on('clientMethodName', (data: any) => {
        });
    
        this.hubConnection.on('WelcomeMethodName', (data: any) => {
          console.log("ConnectionId",data);
          this.hubConnection.invoke('GetDataFromClient', 'abc@abc.com', data).catch((err: any) => console.log(err));
        });
        // Get Messages Notifications:
        this.hubConnection.on('privateMessageMethodName', (data: any) => {
          // Show Notifications in toastr:
          this.showNotification(data);
          // Update List in product table:
          this.productService.get();
          // Update unreading messages:
          setTimeout(() => {
            this.updateUnreadMsgStatus();
          }, 500);
        });
    }

    // Call Toaster FOr Showing Messages:
    showNotification(notification: string) {
      this.toastr.warning( notification);
    }

    // Subscribe to product:
    public subscribeToProduct(productId:string)
    {
      this.hubConnection.invoke("SuscribeToProduct",productId,this.userName)
    }

    // unSubscribe from product:
    public unSubscribeToProduct(productId:string)
    {
      this.hubConnection.invoke("UnSubscribeFromProduct",productId,this.userName)
    }

    // Get List Of UnReading Messages:
    public getUnReadedMsg(userName:string) : Observable<any>{
      return this.http.get(`https://localhost:7132/api/Notfication/getUnreadedNotifications?userName=${userName}`);
    }

    // Get List Of Readed Messages:
    public getReadedMsg(userName:string) : Observable<any>{
      return this.http.post("https://localhost:7132/api/Notfication/getReadedNotifications",userName)
    }

    // Update unreading messages:
    updateUnreadMsgStatus(){
      this.getUnReadedMsg(this.userName).subscribe({
        next : (data:any) => {
        this.unreadMsg.next(data);
      }
      });
    }

    // Set Messages as reading:
    makeMsgAsReading(userName:string) : Observable<any>{
      this.unreadMsg.next([]);
      return this.http.get(`https://localhost:7132/api/Notfication/makeNotificationsAsReaded?userName=${userName}`)
    }

    // Get All Messages for user:
    public getAllMsg(userName:string) : Observable<any>{
      return this.http.get(`https://localhost:7132/api/Notfication/getAllNotificationsList?userName=${userName}`);
    }

    // Get All My Group:
    public getMyGroup(userName:string) : Observable<any>{
      return this.http.get(`https://localhost:7132/api/Notfication/getMyGroups?userName=${userName}`);
    }
}
