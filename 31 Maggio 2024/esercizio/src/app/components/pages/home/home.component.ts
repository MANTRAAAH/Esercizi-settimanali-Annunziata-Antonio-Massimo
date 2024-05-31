import { Component, OnInit } from '@angular/core';
import { TodosService } from '../../../services/todos.service';
import { UsersService } from '../../../services/users.service';
import { Todos } from '../../../models/todos';
import { Users } from '../../../models/users';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  todos: Todos[] = [];
  users: Users[] = [];
  mergedData: any[] = [];
  search = '';

  constructor(private todosService: TodosService, private usersService: UsersService) { }

 ngOnInit() {
  this.todos = this.todosService.getTodos();
  this.users = this.usersService.getUsers();
  this.mergeData();
}

mergeData() {
  if (this.todos && this.todos.length > 0 && this.users && this.users.length > 0) {
    this.mergedData = this.todos.map(todo => {
      return {
        ...todo,
        user: this.users.find(user => user.id === todo.userId)
      };
    });

    console.log(this.mergedData);
  }
}
get filteredData() {
  if (!this.search) {
    return this.mergedData;
  }

  return this.mergedData.filter(item =>
    `${item.user?.firstName} ${item.user?.lastName}`.toLowerCase().includes(this.search.toLowerCase())
  );
}
}
