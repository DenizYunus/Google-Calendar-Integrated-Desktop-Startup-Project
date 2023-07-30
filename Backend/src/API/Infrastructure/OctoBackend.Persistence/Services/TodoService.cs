using AutoMapper;
using MongoDB.Driver;
using OctoBackend.Application.Abstractions.Repositories;
using OctoBackend.Application.Abstractions.Services;
using OctoBackend.Application.Features.Commands.Todo.Add;
using OctoBackend.Application.Features.Commands.Todo.Delete;
using OctoBackend.Application.Features.Commands.Todo.Update;
using OctoBackend.Application.Features.Queries.Todo.GetByCategory;
using OctoBackend.Application.Models;
using OctoBackend.Domain.Collections;
using OctoBackend.Domain.Collections.History;
using OctoBackend.Domain.Constants;
using OctoBackend.Domain.Enums;
using OctoBackend.Domain.Models;
using System.Security.Claims;

namespace OctoBackend.Persistence.Services
{
    public class TodoService : ITodoService
    {
        private readonly IMapper _mapper;
        private readonly ITodoRepository _todoRepository;
        private readonly ITaskHistoryRepository _taskHistoryRepository;
        private readonly IUserRepository _userRepository;

        public TodoService(IMapper mapper, ITodoRepository todoRepository, ITaskHistoryRepository taskHistoryRepository, IUserRepository userRepository)
        {
            _mapper = mapper;
            _todoRepository = todoRepository;
            _taskHistoryRepository = taskHistoryRepository;
            _userRepository = userRepository;
        }

        public async Task<GetOneResponse<GetTodoByCategoryResponse>> GetByCategoryAsync(GetTodoByCategoryQuery query, IEnumerable<Claim> userClaims)
        {
            try
            {
                var userId = userClaims!.Single(x => x.Type == "id").Value;

                var todo = await _todoRepository.GetByCategoryAsync(query.Category, userId);

                if (todo is null)
                    return new() { Message = new("Category does not exists " + query.Category, MessageCode.NotFound) };

                var responseResult = _mapper.Map<GetTodoByCategoryResponse>(todo);

                return new() { Result = responseResult, Success = true };
            }
            catch (Exception ex)
            {
                return new() { Message = new(ex.Message) };
            }
        }

        public async Task<CreateResponse<AddTaskResponse>> AddTaskAsync(AddTaskCommand command, IEnumerable<Claim> userClaims)
        {
            try
            {
                var userId = userClaims!.Single(x => x.Type == "id").Value;

                var todo = await _todoRepository.GetByCategoryAsync(command.Category, userId);

                TaskTodo task = new(command.Task);
                todo.Tasks.Add(task);

                AddTaskResponse response = new()
                {
                    ID = task.ID,
                };

                await _todoRepository.ReplaceOneAsync(todo, todo.Id);

                return new() { Result = response, Success = true };
            }
            catch (Exception ex)
            {
                return new() { Message = new(ex.Message) };
            }        
        }

        public async Task<Response> SetStatusAsync(SetTaskStatusCommand command, IEnumerable<Claim> userClaims)
        {
            try
            {
                var userId = userClaims!.Single(x => x.Type == "id").Value;

                var todo = await _todoRepository.GetByCategoryAsync(command.Category, userId);

                var task = todo.Tasks.FirstOrDefault(i => i.ID == command.TaskID);

                if(task is null)
                    return new() { Message = new("Task with ID: " + command.TaskID + " Does not exist", MessageCode.NotFound) };

                task.Status = TaskStatusConsts.statusTypes[command.Status];

                await _todoRepository.ReplaceOneAsync(todo, todo.Id);

                return new() { Success = true };

            }
            catch (Exception ex)
            {
                return new() { Message = new(ex.Message) };
            }
        }

        public async Task<Response> DeleteCompletedAsync(DeleteCompletedTasksComand command, IEnumerable<Claim> userClaims)
        {
            try
            {
                var userId = userClaims!.Single(x => x.Type == "id").Value;

                UserCollection user = await _userRepository.GetByIdAsync(userId);

                var todo = await _todoRepository.GetByCategoryAsync(command.Category, userId);

                ///TODO: THIS KIND OF EXPRESSIONS CAN BE WRITTEN AT REPO
                var completedTasks = todo.Tasks.Select(dbTask => dbTask).Where(i => i.Status == TodoStatus.Completed).ToList();

                if (completedTasks.Any())
                {
                    List<TaskTodoHistory> historyList;

                    var userInHistory = await _taskHistoryRepository.GetSingleAsync(i => i.UserName == user.UserName);

                    if(userInHistory is null)
                    {
                        historyList = new();
                    }
                    else
                    {
                        historyList = userInHistory.DeletedTasks.ToList();
                    }                   
                    
                    foreach (var task in completedTasks)
                    {
                        todo.Tasks.Remove(task);

                        TaskTodoHistory history = new()
                        {
                            TaskName = task.Content,
                            TaskCategory = todo.Category,
                            CreatedAt = task.CreatedAt,
                            DeletedAt = DateTime.Now,
                        };

                        historyList.Add(history);                       
                    }

                    await _todoRepository.ReplaceOneAsync(todo, todo.Id);

                    if (userInHistory is null)
                    {
                        TaskHistoryCollection historyCollection = new()
                        {
                            UserName = user.UserName!,
                            DeletedTasks = historyList,
                        };

                        await _taskHistoryRepository.InsertOneAsync(historyCollection);
                    }
                    else
                    {
                        userInHistory.DeletedTasks = historyList;
                        await _taskHistoryRepository.ReplaceOneAsync(userInHistory, userInHistory.Id);
                    }
                                                     
                }

                return new() { Success = true };
            }
            catch (Exception ex)
            {
                return new() { Message = new(ex.Message) };
            }
        }

        public async Task<Response> SetDeadlineAsync(SetDeadlineCommand command, IEnumerable<Claim> userClaims)
        {
            try
            {
                var userId = userClaims!.Single(x => x.Type == "id").Value;

                var todo = await _todoRepository.GetByCategoryAsync(command.Category, userId);

                todo.DeadLine = command.Deadline;

                await _todoRepository.ReplaceOneAsync(todo, todo.Id);

                return new() { Success = true };

            }
            catch (Exception ex)
            {
                return new() { Message = new(ex.Message) };
            }
        }
    }
}
