import { Component, Injector, OnInit, ViewEncapsulation } from "@angular/core";
import { AppComponentBase } from "@shared/app-component-base";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import {
  TourServiceProxy,
  GetTourForViewDto,
  TransactionServiceProxy,
  GetTransactionForViewDto
} from "@shared/service-proxies/service-proxies";
import * as moment from "moment";
import { PageEvent } from "@angular/material/paginator";

@Component({
  templateUrl: "./transactions.component.html",
  styleUrls: ['../app.component.less', './transactions.component.less'],
  animations: [appModuleAnimation()],
  encapsulation: ViewEncapsulation.None
})

export class TransactionsComponent extends AppComponentBase implements OnInit {
  public searchQuery: string;
  public transactions: GetTransactionForViewDto[];
  public totalCount: number;
  public maxResultCount: number = 5;
  public skipCount: number = 0;
  public sort: string;
  public maxResultCountOptions: number[] = [1, 5, 10, 25, 100];
  public pageEvent: PageEvent;

  constructor(injector: Injector, private _transactionService: TransactionServiceProxy) {
    super(injector);
  }

  public getTransactions(event?: any): void {
    if (event) {
        this.pageEvent = event;
        this.skipCount = this.pageEvent.pageIndex * this.pageEvent.pageSize;
        this.maxResultCount = this.pageEvent.pageSize;
    }

    this._transactionService
      .getAll(this.searchQuery, this.sort, this.skipCount, this.maxResultCount)
      .subscribe((result) => {
        this.transactions = result.items;
        this.totalCount = result.totalCount;
        console.log("Result")
        console.log(this.transactions)
      });
  }

  ngOnInit(): void {
    this.getTransactions();
  }
}
