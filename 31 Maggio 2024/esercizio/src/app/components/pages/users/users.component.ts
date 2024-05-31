import { Component, OnInit } from '@angular/core';
import { UsersService } from '../../../services/users.service';
import { TodosService } from '../../../services/todos.service';
import { Users } from '../../../models/users';
import { Todos } from '../../../models/todos';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {
  usersWithTodos: { user: Users, todos: Todos[] }[] = [];
  showAllUsers = false;
  constructor(private usersService: UsersService, private todosService: TodosService) { }

  ngOnInit() {
    let users = this.usersService.getUsers();
    let todos = this.todosService.getTodos();

    this.usersWithTodos = users.map(user => {
      return {
        user: user,
        todos: todos.filter(todo => todo.userId === user.id)
      };
    }).filter(userWithTodos => userWithTodos.todos.length > 0);
  }

  toggleShowAllUsers() {
    this.showAllUsers = !this.showAllUsers;
    if (this.showAllUsers) {
      this.usersWithTodos = this.usersService.getUsers().map(user => {
        return {
          user: user,
          todos: this.todosService.getTodos().filter(todo => todo.userId === user.id)
        };
      });
    } else {
      this.usersWithTodos = this.usersService.getUsers().map(user => {
        return {
          user: user,
          todos: this.todosService.getTodos().filter(todo => todo.userId === user.id)
        };
      }).filter(userWithTodos => userWithTodos.todos.length > 0);
    }
  }
}
