import { Component, OnInit } from '@angular/core';
import { TodosService } from '../../../services/todos.service';
import { UsersService } from '../../../services/users.service';
import { Todos } from '../../../models/todos';
import { Users } from '../../../models/users'

@Component({
  selector: 'app-completed',
  templateUrl: './completed.component.html',
  styleUrls: ['./completed.component.scss']
})
export class CompletedComponent implements OnInit {
  completedTodos: { user: Users | undefined, todo: Todos }[] = [];

  constructor(private todosService: TodosService, private usersService: UsersService) { }

  ngOnInit() {
    let todos = this.todosService.getTodos();
    let users = this.usersService.getUsers();

    this.completedTodos = todos.filter(todo => todo.completed).map(todo => {
      return {
        todo: todo,
        user: users.find(user => user.id === todo.userId)
      };
    });
  }
}
