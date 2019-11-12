using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieStore.Controllers;
using MovieStore.Models;
using System.Linq;
using Moq;

namespace MovieStore.Tests.Controllers
{
	using System.Collections.Generic;
	using System.Web.Mvc;
	using System.Data.Entity;
	[TestClass]
	public class MovieControllerTest
	{
		[TestMethod]
		public void MovieStore_Index_TestView()
		{
			MoviesController controller = new MoviesController();

			ViewResult result = controller.Index() as ViewResult;

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void MovieStore_ListOfMovies()
		{
			// Arrange
			MoviesController controller = new MoviesController();

			// Act
			List<Movie> result = controller.ListOfMovies();

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual("Iron Man 1", result[0].Title);
			Assert.AreEqual("Iron Man 2", result[1].Title);
			Assert.AreEqual("Iron Man 3", result[2].Title);
		}

		[TestMethod]
		public void MovieStore_IndexRedirect_Success()
		{
			MoviesController controller = new MoviesController();

			RedirectToRouteResult result = controller.IndexRedirect(id: 1) as RedirectToRouteResult;

			Assert.IsNotNull(result);
			Assert.AreEqual(expected: "Create", actual:result.RouteValues["Action"]);
			Assert.AreEqual(expected: "HomeController", actual: result.RouteValues["controller"]);

		}

		[TestMethod]
		public void MovieStore_IndexRedirect_BadRequest()
		{
			MoviesController controller = new MoviesController();

			HttpStatusCodeResult result = controller.IndexRedirect(id: 0) as HttpStatusCodeResult;

			Assert.AreEqual(expected: HttpStatusCode.BadRequest, actual: (HttpStatusCode)result.StatusCode);
		}

		[TestMethod]
		public void MovieStore_ListFromDb()
		{

			var list = new List<Movie>
			{
				new Movie() {MovieId= 1, Title = "Superman 1" },
				new Movie() {MovieId= 2, Title = "Superman 2" }
			}.AsQueryable();

			Mock<MovieStoreDBContext> mockContext = new Mock<MovieStoreDBContext>();
			Mock<DbSet<Movie>> mockSet = new Mock<DbSet<Movie>>();

			mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.GetEnumerator()).Returns(list.GetEnumerator());
			mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.Provider).Returns(list.Provider);
			mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.ElementType).Returns(list.ElementType);

			mockContext.Setup(expression: db => db.Movies).Returns(mockSet.Object);

			MoviesController controller = new MoviesController(mockContext.Object);

			ViewResult result = controller.ListFromDb() as ViewResult;
			List<Movie> resultMovies = result.Model as List<Movie>;

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void MovieStore_Details_Succcess()
		{

			var list = new List<Movie>
			{
				new Movie() {MovieId= 1, Title = "Superman 1" },
				new Movie() {MovieId= 2, Title = "Superman 2" }
			}.AsQueryable();

			Mock<MovieStoreDBContext> mockContext = new Mock<MovieStoreDBContext>();
			Mock<DbSet<Movie>> mockSet = new Mock<DbSet<Movie>>();

			mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.GetEnumerator()).Returns(list.GetEnumerator());
			mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.Provider).Returns(list.Provider);
			mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.ElementType).Returns(list.ElementType);
			mockSet.Setup(expression: m => m.Find(It.IsAny<Object>())).Returns(list.First());

			mockContext.Setup(expression: db => db.Movies).Returns(mockSet.Object);

			MoviesController controller = new MoviesController(mockContext.Object);

			ViewResult result = controller.Details(id: 1) as ViewResult;



			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void MovieStore_IdIsNull()
		{

			var list = new List<Movie>
			{
				new Movie() {MovieId= 1, Title = "Superman 1" },
				new Movie() {MovieId= 2, Title = "Superman 2" }
			}.AsQueryable();

			Mock<MovieStoreDBContext> mockContext = new Mock<MovieStoreDBContext>();
			Mock<DbSet<Movie>> mockSet = new Mock<DbSet<Movie>>();

			mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.GetEnumerator()).Returns(list.GetEnumerator());
			mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.Provider).Returns(list.Provider);
			mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.ElementType).Returns(list.ElementType);
			mockSet.Setup(expression: m => m.Find(It.IsAny<Object>())).Returns(list.First());

			mockContext.Setup(expression: db => db.Movies).Returns(mockSet.Object);

			MoviesController controller = new MoviesController(mockContext.Object);

			HttpStatusCodeResult result = controller.Details(id: null) as HttpStatusCodeResult;


			Assert.AreEqual(expected: HttpStatusCode.BadRequest, actual: (HttpStatusCode)result.StatusCode);
		}

		[TestMethod]
		public void MovieStore_MovieIsNull()
		{

			var list = new List<Movie>
			{
				new Movie() {MovieId= 1, Title = "Superman 1" },
				new Movie() {MovieId= 2, Title = "Superman 2" }
			}.AsQueryable();

			Mock<MovieStoreDBContext> mockContext = new Mock<MovieStoreDBContext>();
			Mock<DbSet<Movie>> mockSet = new Mock<DbSet<Movie>>();

			mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.GetEnumerator()).Returns(list.GetEnumerator());
			mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.Provider).Returns(list.Provider);
			mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.ElementType).Returns(list.ElementType);

			Movie movie = null;

			mockSet.Setup(expression: m => m.Find(It.IsAny<Object>())).Returns(movie);

			mockContext.Setup(expression: db => db.Movies).Returns(mockSet.Object);

			MoviesController controller = new MoviesController(mockContext.Object);

			HttpStatusCodeResult result = controller.Details(id: 0) as HttpStatusCodeResult;


			Assert.AreEqual(expected: HttpStatusCode.NotFound, actual: (HttpStatusCode)result.StatusCode);
		}

		[TestMethod]
		public void MovieStore_Details_MovieIsNull()
		{
			// Step 1
			var list = new List<Movie>
			{
				new Movie() {MovieId = 1, Title = "Superman 1"},
				new Movie() {MovieId = 2, Title = "Superman 2"}
			}.AsQueryable();

			// Step 2
			Mock<MovieStoreDBContext> mockContext = new Mock<MovieStoreDBContext>();
			Mock<DbSet<Movie>> mockSet = new Mock<DbSet<Movie>>();

			// Step 3
			mockSet.As<IQueryable<Movie>>().Setup(m => m.GetEnumerator()).Returns(list.GetEnumerator());
			mockSet.As<IQueryable<Movie>>().Setup(m => m.Provider).Returns(list.Provider);
			mockSet.As<IQueryable<Movie>>().Setup(m => m.ElementType).Returns(list.ElementType);

			Movie movie = null;

			mockSet.Setup(m => m.Find(It.IsAny<Object>())).Returns(movie);

			// Step 4
			mockContext.Setup(db => db.Movies).Returns(mockSet.Object);

			// Arrange
			MoviesController controller = new MoviesController(mockContext.Object);

			// Act
			HttpStatusCodeResult result = controller.Details(0) as HttpStatusCodeResult;

			// Assert
			Assert.AreEqual(HttpStatusCode.NotFound, (HttpStatusCode)result.StatusCode);
		}

		[TestMethod]
		public void MovieStore_Create_TestView()
		{
			// Arrange
			MoviesController controller = new MoviesController();

			// Act
			ViewResult result = controller.Create() as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void MovieStore_CreatePOST_Success()
		{
			// Step 1
			Mock<MovieStoreDBContext> mockContext = new Mock<MovieStoreDBContext>();
			Mock<DbSet<Movie>> mockSet = new Mock<DbSet<Movie>>();

			// Step 2
			var movie = new Movie()
			{
				MovieId = 1,
				Title = "Superman",
				YearRelease = 2018
			};

			// Step 3
			mockContext.Setup(db => db.Movies).Returns(mockSet.Object);

			// Arrange
			MoviesController controller = new MoviesController(mockContext.Object);

			// Act
			RedirectToRouteResult result = controller.Create(movie) as RedirectToRouteResult;

			// Assert
			Assert.AreEqual("Index", result.RouteValues["action"]);
		}

		[TestMethod]
		public void MovieStore_CreatePOST_NotValid()
		{
			// Step 1
			Mock<MovieStoreDBContext> mockContext = new Mock<MovieStoreDBContext>();
			Mock<DbSet<Movie>> mockSet = new Mock<DbSet<Movie>>();

			// Step 2
			var movie = new Movie()
			{
				MovieId = 1,
				Title = "Superman",
				YearRelease = 2018
			};

			// Step 3
			mockContext.Setup(db => db.Movies).Returns(mockSet.Object);

			// Arrange
			MoviesController controller = new MoviesController(mockContext.Object);

			// Act
			controller.ModelState.AddModelError("test", "test");

			ViewResult result = controller.Create(movie) as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void MovieStore_EditGET_Success()
		{
			// Step 1
			var list = new List<Movie>
			{
				new Movie() {MovieId = 1, Title = "Superman 1"},
				new Movie() {MovieId = 2, Title = "Superman 2"}
			}.AsQueryable();

			// Step 2
			Mock<MovieStoreDBContext> mockContext = new Mock<MovieStoreDBContext>();
			Mock<DbSet<Movie>> mockSet = new Mock<DbSet<Movie>>();

			// Step 3
			mockSet.As<IQueryable<Movie>>().Setup(m => m.GetEnumerator()).Returns(list.GetEnumerator());
			mockSet.As<IQueryable<Movie>>().Setup(m => m.Provider).Returns(list.Provider);
			mockSet.As<IQueryable<Movie>>().Setup(m => m.ElementType).Returns(list.ElementType);
			mockSet.Setup(m => m.Find(It.IsAny<Object>())).Returns(list.First());

			// Step 4
			mockContext.Setup(db => db.Movies).Returns(mockSet.Object);

			// Arrange
			MoviesController controller = new MoviesController(mockContext.Object);

			// Act
			ViewResult result = controller.Edit(1) as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void MovieStore_EditGET_IdIsNull()
		{
			// Step 1
			var list = new List<Movie>
			{
				new Movie() {MovieId = 1, Title = "Superman 1"},
				new Movie() {MovieId = 2, Title = "Superman 2"}
			}.AsQueryable();

			// Step 2
			Mock<MovieStoreDBContext> mockContext = new Mock<MovieStoreDBContext>();
			Mock<DbSet<Movie>> mockSet = new Mock<DbSet<Movie>>();

			// Step 3
			mockSet.As<IQueryable<Movie>>().Setup(m => m.GetEnumerator()).Returns(list.GetEnumerator());
			mockSet.As<IQueryable<Movie>>().Setup(m => m.Provider).Returns(list.Provider);
			mockSet.As<IQueryable<Movie>>().Setup(m => m.ElementType).Returns(list.ElementType);
			mockSet.Setup(m => m.Find(It.IsAny<Object>())).Returns(list.First());

			// Step 4
			mockContext.Setup(db => db.Movies).Returns(mockSet.Object);

			// Arrange
			MoviesController controller = new MoviesController(mockContext.Object);

			// Act
			HttpStatusCodeResult result = controller.Edit((int?)null) as HttpStatusCodeResult;

			// Assert
			Assert.AreEqual(HttpStatusCode.BadRequest, (HttpStatusCode)result.StatusCode);
		}

		[TestMethod]
		public void MovieStore_EditGET_MovieIsNull()
		{
			// Goal: Query from our own list instead of the database.

			// Step 1
			var list = new List<Movie>
			{
				new Movie() {MovieId = 1, Title = "Superman 1"},
				new Movie() {MovieId = 2, Title = "Superman 2"}
			}.AsQueryable();

			// Step 2
			Mock<MovieStoreDBContext> mockContext = new Mock<MovieStoreDBContext>();
			Mock<DbSet<Movie>> mockSet = new Mock<DbSet<Movie>>();

			// Step 3
			mockSet.As<IQueryable<Movie>>().Setup(m => m.GetEnumerator()).Returns(list.GetEnumerator());
			mockSet.As<IQueryable<Movie>>().Setup(m => m.Provider).Returns(list.Provider);
			mockSet.As<IQueryable<Movie>>().Setup(m => m.ElementType).Returns(list.ElementType);

			Movie movie = null;

			mockSet.Setup(m => m.Find(It.IsAny<Object>())).Returns(movie);

			// Step 4
			mockContext.Setup(db => db.Movies).Returns(mockSet.Object);

			// Arrange
			MoviesController controller = new MoviesController(mockContext.Object);

			// Act
			HttpStatusCodeResult result = controller.Edit(0) as HttpStatusCodeResult;

			// Assert
			Assert.AreEqual(HttpStatusCode.NotFound, (HttpStatusCode)result.StatusCode);
		}

		[TestMethod]
		public void MovieStore_EditPOST_Success()
		{
			// Step 1
			Mock<MovieStoreDBContext> mockContext = new Mock<MovieStoreDBContext>();
			Mock<DbSet<Movie>> mockSet = new Mock<DbSet<Movie>>();

			// Step 2
			var movie = new Movie()
			{
				MovieId = 1,
				Title = "Superman",
				YearRelease = 2018
			};

			// Step 3
			mockContext.Setup(db => db.Movies).Returns(mockSet.Object);

			// Arrange
			MoviesController controller = new MoviesController(mockContext.Object);

			// Act
			RedirectToRouteResult result = controller.Create(movie) as RedirectToRouteResult;

			// Assert
			Assert.AreEqual("Index", result.RouteValues["action"]);
		}

		[TestMethod]
		public void MovieStore_EditPOST_NotValid()
		{
			// Step 1
			Mock<MovieStoreDBContext> mockContext = new Mock<MovieStoreDBContext>();
			Mock<DbSet<Movie>> mockSet = new Mock<DbSet<Movie>>();

			// Step 2
			var movie = new Movie()
			{
				MovieId = 1,
				Title = "Superman",
				YearRelease = 2018
			};

			// Step 3
			mockContext.Setup(db => db.Movies).Returns(mockSet.Object);

			// Arrange
			MoviesController controller = new MoviesController(mockContext.Object);

			// Act
			controller.ModelState.AddModelError("test", "test");

			ViewResult result = controller.Edit(movie) as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void MovieStore_DeleteGET_Success()
		{
			// Step 1
			var list = new List<Movie>
			{
				new Movie() {MovieId = 1, Title = "Superman 1"},
				new Movie() {MovieId = 2, Title = "Superman 2"}
			}.AsQueryable();

			// Step 2
			Mock<MovieStoreDBContext> mockContext = new Mock<MovieStoreDBContext>();
			Mock<DbSet<Movie>> mockSet = new Mock<DbSet<Movie>>();

			// Step 3
			mockSet.As<IQueryable<Movie>>().Setup(m => m.GetEnumerator()).Returns(list.GetEnumerator());
			mockSet.As<IQueryable<Movie>>().Setup(m => m.Provider).Returns(list.Provider);
			mockSet.As<IQueryable<Movie>>().Setup(m => m.ElementType).Returns(list.ElementType);
			mockSet.Setup(m => m.Find(It.IsAny<Object>())).Returns(list.First());

			// Step 4
			mockContext.Setup(db => db.Movies).Returns(mockSet.Object);

			// Arrange
			MoviesController controller = new MoviesController(mockContext.Object);

			// Act
			ViewResult result = controller.Delete(1) as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void MovieStore_DeleteGET_IdIsNull()
		{
			// Goal: Query from our own list instead of the database.

			// Step 1
			var list = new List<Movie>
			{
				new Movie() {MovieId = 1, Title = "Superman 1"},
				new Movie() {MovieId = 2, Title = "Superman 2"}
			}.AsQueryable();

			// Step 2
			Mock<MovieStoreDBContext> mockContext = new Mock<MovieStoreDBContext>();
			Mock<DbSet<Movie>> mockSet = new Mock<DbSet<Movie>>();

			// Step 3
			mockSet.As<IQueryable<Movie>>().Setup(m => m.GetEnumerator()).Returns(list.GetEnumerator());
			mockSet.As<IQueryable<Movie>>().Setup(m => m.Provider).Returns(list.Provider);
			mockSet.As<IQueryable<Movie>>().Setup(m => m.ElementType).Returns(list.ElementType);
			mockSet.Setup(m => m.Find(It.IsAny<Object>())).Returns(list.First());

			// Step 4
			mockContext.Setup(db => db.Movies).Returns(mockSet.Object);

			// Arrange
			MoviesController controller = new MoviesController(mockContext.Object);

			// Act
			HttpStatusCodeResult result = controller.Delete(null) as HttpStatusCodeResult;

			// Assert
			Assert.AreEqual(HttpStatusCode.BadRequest, (HttpStatusCode)result.StatusCode);
		}

		[TestMethod]
		public void MovieStore_DeleteGET_MovieIsNull()
		{
			// Goal: Query from our own list instead of the database.

			// Step 1
			var list = new List<Movie>
			{
				new Movie() {MovieId = 1, Title = "Superman 1"},
				new Movie() {MovieId = 2, Title = "Superman 2"}
			}.AsQueryable();

			// Step 2
			Mock<MovieStoreDBContext> mockContext = new Mock<MovieStoreDBContext>();
			Mock<DbSet<Movie>> mockSet = new Mock<DbSet<Movie>>();

			// Step 3
			mockSet.As<IQueryable<Movie>>().Setup(m => m.GetEnumerator()).Returns(list.GetEnumerator());
			mockSet.As<IQueryable<Movie>>().Setup(m => m.Provider).Returns(list.Provider);
			mockSet.As<IQueryable<Movie>>().Setup(m => m.ElementType).Returns(list.ElementType);

			Movie movie = null;

			mockSet.Setup(m => m.Find(It.IsAny<Object>())).Returns(movie);

			// Step 4
			mockContext.Setup(db => db.Movies).Returns(mockSet.Object);

			// Arrange
			MoviesController controller = new MoviesController(mockContext.Object);

			// Act
			HttpStatusCodeResult result = controller.Delete(0) as HttpStatusCodeResult;

			// Assert
			Assert.AreEqual(HttpStatusCode.NotFound, (HttpStatusCode)result.StatusCode);
		}

		[TestMethod]
		public void MovieStore_DeletePOST_Success()
		{
			// Step 1
			Mock<MovieStoreDBContext> mockContext = new Mock<MovieStoreDBContext>();
			Mock<DbSet<Movie>> mockSet = new Mock<DbSet<Movie>>();

			// Step 2
			mockContext.Setup(db => db.Movies).Returns(mockSet.Object);

			// Arrange
			MoviesController controller = new MoviesController(mockContext.Object);

			// Act
			RedirectToRouteResult result = controller.DeleteConfirmed(1) as RedirectToRouteResult;

			// Assert
			Assert.AreEqual("Index", result.RouteValues["action"]);
		}
	}
}
