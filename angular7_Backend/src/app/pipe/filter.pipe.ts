import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filter'

})
export class FilterPipe implements PipeTransform {

  transform(items: any[], searchText: string, searchCols: any[]): any[] {

    // 確認傳入條件
    if (!items || items.length == 0) {
      return [];
    } else if (!searchText) {
      return items;
    }

    // 一律轉為小寫進行比對
    searchText = searchText.toLowerCase();
    // 比對邏輯
    return items.filter(a => {
      // 預設 false
      var result = false;

      // 加入比對條件，searchCols(要搜尋的欄位名稱)

      for (let index = 0; index < searchCols.length; index++) {
        const element = searchCols[index];
        result = result || Boolean(String(a[element]).toLowerCase().includes(searchText));
      }

      return result;
    });
  }
}
