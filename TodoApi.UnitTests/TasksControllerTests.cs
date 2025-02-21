using Moq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Repositories;
using TodoApi.Controllers;
using TodoApi.Models.Entities;
using TodoApi.Models.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TodoApi.UnitTests;

public class TasksControllerTests
{
	private readonly Mock<ITaskRepository> _mockTaskRepository;
	private readonly Mock<IUserRepository> _mockUserRepository;
	private readonly TasksController _tasksController;

	public TasksControllerTests()
	{
		_mockTaskRepository = new Mock<ITaskRepository>();
		_mockUserRepository = new Mock<IUserRepository>();
		_tasksController = new TasksController(_mockTaskRepository.Object, _mockUserRepository.Object);
	}

	[Fact]
	public async Task GetTasks_ReturnsOkObjectResult()
	{
		// Arrange
		var mockUser = new User
		{
			Id = 1,
			Email = "Test Email"
		};
		var mockTasks = new List<TodoTask>
		{
			new TodoTask
			{
				Id = 1,
				Name = "Test Task",
				Description = "Test Description",
				User = new User { Id = 1, Email = "Test Email" },
				CreatedAt = DateTime.UtcNow,
				Completed = false
			}
		};
		_mockTaskRepository.Setup(repo => repo.GetTasks(1)).ReturnsAsync(mockTasks);
		_mockUserRepository.Setup(repo => repo.GetUser(1)).ReturnsAsync(mockUser);
		var user = new ClaimsPrincipal(new ClaimsIdentity(
		[
			new Claim("userId", "1"),
			new Claim(ClaimTypes.Email, "Test Email")
		], "mock"));
		_tasksController.ControllerContext = new ControllerContext
		{
			HttpContext = new DefaultHttpContext { User = user }
		};

		// Act
		var result = await _tasksController.GetTasks();

		// Assert
		var okResult = Assert.IsType<OkObjectResult>(result);
		var tasks = Assert.IsType<List<TodoTask>>(okResult.Value);
		Assert.Equal(mockTasks.Count, tasks.Count);
	}

	[Fact]
	public async Task GetTasks_ReturnsUnauthorizedObjectResultIfMissingClaimsPrincipal()
	{
		// Arrange
		var mockUser = new User
		{
			Id = 1,
			Email = "Test Email"
		};
		var mockTasks = new List<TodoTask>
		{
			new TodoTask
			{
				Id = 1,
				Name = "Test Task",
				Description = "Test Description",
				User = new User { Id = 1, Email = "Test Email" },
				CreatedAt = DateTime.UtcNow,
				Completed = false
			}
		};
		_mockTaskRepository.Setup(repo => repo.GetTasks(1)).ReturnsAsync(mockTasks);
		_mockUserRepository.Setup(repo => repo.GetUser(1)).ReturnsAsync(mockUser);

		// Act
		_tasksController.ControllerContext = new ControllerContext
		{
			HttpContext = new DefaultHttpContext() // No User set
		};
		var result = await _tasksController.GetTasks();

		// Assert
		var okResult = Assert.IsType<UnauthorizedObjectResult>(result);
	}

	[Fact]	
	public async Task GetTasks_ReturnsBadRequestIfInvalidUserIdClaim()
	{
		// Arrange
		var mockUser = new User
		{
			Id = 1,
			Email = "Test Email"
		};
		var mockTasks = new List<TodoTask>
		{
			new TodoTask
			{
				Id = 1,
				Name = "Test Task",
				Description = "Test Description",
				User = new User { Id = 1, Email = "Test Email" },
				CreatedAt = DateTime.UtcNow,
				Completed = false
			}
		};
		_mockTaskRepository.Setup(repo => repo.GetTasks(1)).ReturnsAsync(mockTasks);
		_mockUserRepository.Setup(repo => repo.GetUser(1)).ReturnsAsync(mockUser);

		var user = new ClaimsPrincipal(new ClaimsIdentity(
		[
			new Claim("userId", "string instead of int"),
			new Claim(ClaimTypes.Email, "Test Email")
		], "mock"));
		_tasksController.ControllerContext = new ControllerContext
		{
			HttpContext = new DefaultHttpContext { User = user }
		};
		var result = await _tasksController.GetTasks();

		// Assert
		var okResult = Assert.IsType<BadRequestObjectResult>(result);
	}
	
	[Fact]
	public async Task GetTasks_ReturnsNotFoundIfUserIsNull()
	{
		_mockUserRepository.Setup(repo => repo.GetUser(1)).ReturnsAsync((User) null);

		var user = new ClaimsPrincipal(new ClaimsIdentity(
		[
			new Claim("userId", "1"),
			new Claim(ClaimTypes.Email, "Test Email")
		], "mock"));
		_tasksController.ControllerContext = new ControllerContext
		{
			HttpContext = new DefaultHttpContext { User = user }
		};

		// Act
		var result = await _tasksController.GetTasks();

		// Assert
		var okResult = Assert.IsType<NotFoundObjectResult>(result);
	}

	[Fact]
	public async Task GetTask_ReturnsOkObjectResult()
	{
		// Arrange
		var mockUser = new User
		{
			Id = 1,
			Email = "Test Email"
		};
		var mockTask = new TodoTask
		{
			Id = 1,
			Name = "Test Task",
			Description = "Test Description",
			User = new User { Id = 1, Email = "Test Email" },
			CreatedAt = DateTime.UtcNow,
			Completed = false
		};
		_mockTaskRepository.Setup(repo => repo.GetTask(1)).ReturnsAsync(mockTask);
		_mockUserRepository.Setup(repo => repo.GetUser(1)).ReturnsAsync(mockUser);
		var user = new ClaimsPrincipal(new ClaimsIdentity(
		[
			new Claim("userId", "1"),
			new Claim(ClaimTypes.Email, "Test Email")
		], "mock"));
		_tasksController.ControllerContext = new ControllerContext
		{
			HttpContext = new DefaultHttpContext { User = user }
		};

		// Act
		var result = await _tasksController.GetTask(1);

		// Assert
		var okResult = Assert.IsType<OkObjectResult>(result);
		var tasks = Assert.IsType<TodoTask>(okResult.Value);
	}

	[Fact]
	public async Task GetTask_ReturnsUnauthorizedObjectResultIfMissingClaimsPrincipal()
	{
		// Arrange
		var mockUser = new User
		{
			Id = 1,
			Email = "Test Email"
		};
		var mockTask = new TodoTask
		{
			Id = 1,
			Name = "Test Task",
			Description = "Test Description",
			User = new User { Id = 1, Email = "Test Email" },
			CreatedAt = DateTime.UtcNow,
			Completed = false
		};
		_mockTaskRepository.Setup(repo => repo.GetTask(1)).ReturnsAsync(mockTask);
		_mockUserRepository.Setup(repo => repo.GetUser(1)).ReturnsAsync(mockUser);

		// Act
		_tasksController.ControllerContext = new ControllerContext
		{
			HttpContext = new DefaultHttpContext() // No User set
		};
		var result = await _tasksController.GetTask(1);

		// Assert
		var okResult = Assert.IsType<UnauthorizedObjectResult>(result);
	}
	
	[Fact]	
	public async Task GetTask_ReturnsBadRequestIfInvalidUserIdClaim()
	{
		// Arrange
		var mockUser = new User
		{
			Id = 1,
			Email = "Test Email"
		};
		var mockTask = new TodoTask
		{
			Id = 1,
			Name = "Test Task",
			Description = "Test Description",
			User = new User { Id = 1, Email = "Test Email" },
			CreatedAt = DateTime.UtcNow,
			Completed = false
		};
		_mockTaskRepository.Setup(repo => repo.GetTask(1)).ReturnsAsync(mockTask);
		_mockUserRepository.Setup(repo => repo.GetUser(1)).ReturnsAsync(mockUser);

		var user = new ClaimsPrincipal(new ClaimsIdentity(
		[
			new Claim("userId", "string instead of int"),
			new Claim(ClaimTypes.Email, "Test Email")
		], "mock"));
		_tasksController.ControllerContext = new ControllerContext
		{
			HttpContext = new DefaultHttpContext { User = user }
		};

		// Act
		var result = await _tasksController.GetTask(1);

		// Assert
		var okResult = Assert.IsType<BadRequestObjectResult>(result);
	}	

	[Fact]
	public async Task GetTask_ReturnsNotFoundIfUserIsNull()
	{
		// Arrange
		_mockUserRepository.Setup(repo => repo.GetUser(1)).ReturnsAsync((User)null);
		var user = new ClaimsPrincipal(new ClaimsIdentity(
		[
			new Claim("userId", "1"),
			new Claim(ClaimTypes.Email, "Test Email")
		], "mock"));
		_tasksController.ControllerContext = new ControllerContext
		{
			HttpContext = new DefaultHttpContext { User = user }
		};

		// Act
		var result = await _tasksController.GetTask(1);

		// Assert
		Assert.IsType<NotFoundObjectResult>(result);
	}

	[Fact]
	public async Task AddTask_ReturnsCreatedAtActionResult()
	{
		// Arrange
		var mockUser = new User
		{
			Id = 1,
			Email = "Test Email"
		};
		var newTask = new AddTaskDto("Test Task", "Test Description");
		var task = new TodoTask
		{
			Id = 1,
			Name = "Test Task",
			Description = "Test Description",
			User = new User { Id = 1, Email = "Test Email" },
			CreatedAt = DateTime.UtcNow,
			Completed = false
		};
		_mockUserRepository.Setup(repo => repo.GetUser(1)).ReturnsAsync(mockUser);
		_mockTaskRepository.Setup(repo => repo.AddTask(It.IsAny<TodoTask>())).Callback<TodoTask>(t => t.Id = 1).ReturnsAsync(task);

		var user = new ClaimsPrincipal(new ClaimsIdentity(
		[
			new Claim("userId", "1"),
			new Claim(ClaimTypes.Email, "Test Email")
		], "mock"));
		_tasksController.ControllerContext = new ControllerContext
		{
			HttpContext = new DefaultHttpContext { User = user }
		};

		// Act
		var result = await _tasksController.AddTask(newTask);

		// Assert
		var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
		var actualTask = Assert.IsType<TodoTask>(createdAtActionResult.Value);
		Assert.Equal(task.Id, actualTask.Id);
		Assert.Equal(task.Name, actualTask.Name);
	}

	[Fact]
	public async Task AddTask_ReturnsUnauthorizedObjectResultIfMissingClaimsPrincipal()
	{
		// Arrange
		var mockUser = new User
		{
			Id = 1,
			Email = "Test Email"
		};
		var newTask = new AddTaskDto("Test Task", "Test Description");
		var task = new TodoTask
		{
			Id = 1,
			Name = "Test Task",
			Description = "Test Description",
			User = new User { Id = 1, Email = "Test Email" },
			CreatedAt = DateTime.UtcNow,
			Completed = false
		};
		_mockUserRepository.Setup(repo => repo.GetUser(1)).ReturnsAsync(mockUser);
		_mockTaskRepository.Setup(repo => repo.AddTask(It.IsAny<TodoTask>())).Callback<TodoTask>(t => t.Id = 1).ReturnsAsync(task);

		_tasksController.ControllerContext = new ControllerContext
		{
			HttpContext = new DefaultHttpContext() // No User set
		};

		// Act
		var result = await _tasksController.AddTask(newTask);

		// Assert
		var okResult = Assert.IsType<UnauthorizedObjectResult>(result);
	}

	[Fact]
	public async Task AddTask_ReturnsBadRequestIfInvalidUserClaim()
	{
		// Arrange
		var mockUser = new User
		{
			Id = 1,
			Email = "Test Email"
		};
		var newTask = new AddTaskDto("Test Task", "Test Description");
		var task = new TodoTask
		{
			Id = 1,
			Name = "Test Task",
			Description = "Test Description",
			User = new User { Id = 1, Email = "Test Email" },
			CreatedAt = DateTime.UtcNow,
			Completed = false
		};
		_mockUserRepository.Setup(repo => repo.GetUser(1)).ReturnsAsync(mockUser);
		_mockTaskRepository.Setup(repo => repo.AddTask(It.IsAny<TodoTask>())).Callback<TodoTask>(t => t.Id = 1).ReturnsAsync(task);

		var user = new ClaimsPrincipal(new ClaimsIdentity(
		[
			new Claim("userId", "string instead of int"),
			new Claim(ClaimTypes.Email, "Test Email")
		], "mock"));
		_tasksController.ControllerContext = new ControllerContext
		{
			HttpContext = new DefaultHttpContext { User = user }
		};

		// Act
		var result = await _tasksController.AddTask(newTask);

		// Assert
		var okResult = Assert.IsType<BadRequestObjectResult>(result);
	}

	[Fact]
	public async Task AddTask_ReturnsNotFoundIfUserIsNull()
	{
		// Arrange
		var newTask = new AddTaskDto("Test Task", "Test Description");
		_mockUserRepository.Setup(repo => repo.GetUser(1)).ReturnsAsync((User)null);

		var user = new ClaimsPrincipal(new ClaimsIdentity(
		[
			new Claim("userId", "1"),
			new Claim(ClaimTypes.Email, "Test Email")
		], "mock"));
		_tasksController.ControllerContext = new ControllerContext
		{
			HttpContext = new DefaultHttpContext { User = user }
		};

		// Act
		var result = await _tasksController.AddTask(newTask);

		// Assert
		Assert.IsType<NotFoundObjectResult>(result);
	}

	[Fact]
	public async Task UpdateTask_ReturnsOkObjectResult()
	{
		// Arrange
		var mockUser = new User
		{
			Id = 1,
			Email = "Test Email"
		};
		var newTask = new EditTaskDto(1, "Test Task", "Test Description", false);
		var task = new TodoTask
		{
			Id = 1,
			Name = "Test Task",
			Description = "Test Description",
			User = new User { Id = 1, Email = "Test Email" },
			CreatedAt = DateTime.UtcNow,
			Completed = false
		};
		_mockUserRepository.Setup(repo => repo.GetUser(1)).ReturnsAsync(mockUser);
		_mockTaskRepository.Setup(repo => repo.EditTask(It.IsAny<TodoTask>())).ReturnsAsync(task);
		_mockTaskRepository.Setup(repo => repo.GetTask(1)).ReturnsAsync(task);

		var user = new ClaimsPrincipal(new ClaimsIdentity(
		[
			new Claim("userId", "1"),
			new Claim(ClaimTypes.Email, "Test Email")
		], "mock"));
		_tasksController.ControllerContext = new ControllerContext
		{
			HttpContext = new DefaultHttpContext { User = user }
		};

		// Act
		var result = await _tasksController.UpdateTask(newTask);

		// Assert
		var okResult = Assert.IsType<OkObjectResult>(result);
		var actualTask = Assert.IsType<TodoTask>(okResult.Value);
		Assert.Equal(task.Id, actualTask.Id);
		Assert.Equal(task.Name, actualTask.Name);
	}

	[Fact]
	public async Task UpdateTask_ReturnsUnauthorizedObjectResultIfMissingClaimsPrincipal()
	{
		// Arrange
		var mockUser = new User
		{
			Id = 1,
			Email = "Test Email"
		};
		var newTask = new EditTaskDto(1, "Test Task", "Test Description", false);
		var task = new TodoTask
		{
			Id = 1,
			Name = "Test Task",
			Description = "Test Description",
			User = new User { Id = 1, Email = "Test Email" },
			CreatedAt = DateTime.UtcNow,
			Completed = false
		};
		_mockUserRepository.Setup(repo => repo.GetUser(1)).ReturnsAsync(mockUser);
		_mockTaskRepository.Setup(repo => repo.EditTask(It.IsAny<TodoTask>())).ReturnsAsync(task);
		_mockTaskRepository.Setup(repo => repo.GetTask(1)).ReturnsAsync(task);

		_tasksController.ControllerContext = new ControllerContext
		{
			HttpContext = new DefaultHttpContext() // No User set
		};

		// Act
		var result = await _tasksController.UpdateTask(newTask);

		// Assert
		Assert.IsType<UnauthorizedObjectResult>(result);
	}

	[Fact]
	public async Task UpdateTask_ReturnsBadRequestIfInvalidUserClaim()
	{
		// Arrange
		var mockUser = new User
		{
			Id = 1,
			Email = "Test Email"
		};
		var newTask = new EditTaskDto(1, "Test Task", "Test Description", false);
		var task = new TodoTask
		{
			Id = 1,
			Name = "Test Task",
			Description = "Test Description",
			User = new User { Id = 1, Email = "Test Email" },
			CreatedAt = DateTime.UtcNow,
			Completed = false
		};
		_mockUserRepository.Setup(repo => repo.GetUser(1)).ReturnsAsync(mockUser);
		_mockTaskRepository.Setup(repo => repo.EditTask(It.IsAny<TodoTask>())).ReturnsAsync(task);
		_mockTaskRepository.Setup(repo => repo.GetTask(1)).ReturnsAsync(task);


		var user = new ClaimsPrincipal(new ClaimsIdentity(
		[
			new Claim("userId", "string instead of int"),
			new Claim(ClaimTypes.Email, "Test Email")
		], "mock"));
		_tasksController.ControllerContext = new ControllerContext
		{
			HttpContext = new DefaultHttpContext { User = user }
		};

		// Act
		var result = await _tasksController.UpdateTask(newTask);

		// Assert
		Assert.IsType<BadRequestObjectResult>(result);
	}

	[Fact]
	public async Task UpdateTask_ReturnsNotFoundIfUserIsNull()
	{
		// Arrange
		var newTask = new EditTaskDto(1, "Test Task", "Test Description", false);
		_mockUserRepository.Setup(repo => repo.GetUser(1)).ReturnsAsync((User)null);

		var user = new ClaimsPrincipal(new ClaimsIdentity(
		[
			new Claim("userId", "1"),
			new Claim(ClaimTypes.Email, "Test Email")
		], "mock"));
		_tasksController.ControllerContext = new ControllerContext
		{
			HttpContext = new DefaultHttpContext { User = user }
		};

		// Act
		var result = await _tasksController.UpdateTask(newTask);

		// Assert
		Assert.IsType<NotFoundObjectResult>(result);
	}

	[Fact]
	public async Task DeleteTask_ReturnsOkResult()
	{
		// Arrange
		var mockUser = new User
		{
			Id = 1,
			Email = "Test Email"
		};
		var task = new TodoTask
		{
			Id = 1,
			Name = "Test Task",
			Description = "Test Description",
			User = new User { Id = 1, Email = "Test Email" },
			CreatedAt = DateTime.UtcNow,
			Completed = false
		};
		_mockUserRepository.Setup(repo => repo.GetUser(1)).ReturnsAsync(mockUser);
		_mockTaskRepository.Setup(repo => repo.GetTask(1)).ReturnsAsync(task);

		var user = new ClaimsPrincipal(new ClaimsIdentity(
		[
			new Claim("userId", "1"),
			new Claim(ClaimTypes.Email, "Test Email")
		], "mock"));
		_tasksController.ControllerContext = new ControllerContext
		{
			HttpContext = new DefaultHttpContext { User = user }
		};

		// Act
		var result = await _tasksController.DeleteTask(1);

		// Assert
		Assert.IsType<OkResult>(result);
	}

	[Fact]
	public async Task DeleteTask_ReturnsUnauthorizedResultIfMissingClaimsPrincipal()
	{
		// Arrange
		var mockUser = new User
		{
			Id = 1,
			Email = "Test Email"
		};
		var task = new TodoTask
		{
			Id = 1,
			Name = "Test Task",
			Description = "Test Description",
			User = new User { Id = 1, Email = "Test Email" },
			CreatedAt = DateTime.UtcNow,
			Completed = false
		};
		_mockUserRepository.Setup(repo => repo.GetUser(1)).ReturnsAsync(mockUser);
		_mockTaskRepository.Setup(repo => repo.GetTask(1)).ReturnsAsync(task);

		_tasksController.ControllerContext = new ControllerContext
		{
			HttpContext = new DefaultHttpContext() // No User set
		};

		// Act
		var result = await _tasksController.DeleteTask(1);

		// Assert
		Assert.IsType<UnauthorizedObjectResult>(result);
	}

	[Fact]
	public async Task DeleteTask_ReturnsBadRequestIfInvalidUserClaim()
	{
			// Arrange
		var mockUser = new User
		{
			Id = 1,
			Email = "Test Email"
		};
		var task = new TodoTask
		{
			Id = 1,
			Name = "Test Task",
			Description = "Test Description",
			User = new User { Id = 1, Email = "Test Email" },
			CreatedAt = DateTime.UtcNow,
			Completed = false
		};
		_mockUserRepository.Setup(repo => repo.GetUser(1)).ReturnsAsync(mockUser);
		_mockTaskRepository.Setup(repo => repo.GetTask(1)).ReturnsAsync(task);

		var user = new ClaimsPrincipal(new ClaimsIdentity(
		[
			new Claim("userId", "string instead of int"),
			new Claim(ClaimTypes.Email, "Test Email")
		], "mock"));
		_tasksController.ControllerContext = new ControllerContext
		{
			HttpContext = new DefaultHttpContext { User = user }
		};

		// Act
		var result = await _tasksController.DeleteTask(1);

		// Assert
		Assert.IsType<BadRequestObjectResult>(result);	
	}

	[Fact]
	public async Task DeleteTask_ReturnsNotFoundIfUserIsNull()
	{
		// Arrange
		_mockUserRepository.Setup(repo => repo.GetUser(1)).ReturnsAsync((User)null);

		var user = new ClaimsPrincipal(new ClaimsIdentity(
		[
			new Claim("userId", "1"),
			new Claim(ClaimTypes.Email, "Test Email")
		], "mock"));
		_tasksController.ControllerContext = new ControllerContext
		{
			HttpContext = new DefaultHttpContext { User = user }
		};

		// Act
		var result = await _tasksController.DeleteTask(1);

		// Assert
		Assert.IsType<NotFoundObjectResult>(result);
	}
}
