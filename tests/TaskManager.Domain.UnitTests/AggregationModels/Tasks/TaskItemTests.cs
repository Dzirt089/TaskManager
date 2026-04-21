#nullable enable
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManager.Domain.AggregationModels.Tasks;
using TMTaskStatus = TaskManager.Domain.AggregationModels.Tasks.TaskStatus;
using TaskManager.Domain.Exceptions;

namespace TaskManager.Domain.UnitTests.AggregationModels.Tasks
{
    [TestClass]
    public class TaskItemTests
    {
        [TestMethod]
        public void Constructor_ValidInputs_SetsProperties()
        {
            // Arrange
            var before = DateTime.UtcNow;
            var title = "  My Title  ";
            var description = "  description  ";
            var taskTypeId = Guid.NewGuid();
            var due = DateTime.UtcNow.AddDays(3);

            // Act
            var item = new TaskItem(title, description, taskTypeId, due);
            var after = DateTime.UtcNow;

            // Assert
            Assert.AreNotEqual(Guid.Empty, item.Id);
            Assert.AreEqual("My Title", item.Title);
            Assert.AreEqual("description", item.Description);
            Assert.AreEqual(taskTypeId, item.TaskTypeId);
            Assert.AreEqual(due, item.DueDateUtc);
            Assert.AreEqual(TMTaskStatus.New, item.Status);
            Assert.IsTrue(item.CreatedAtUtc >= before && item.CreatedAtUtc <= after, "CreatedAtUtc should be set to now");
            Assert.IsNull(item.UpdatedAtUtc);
        }

        [TestMethod]
        public void Constructor_TaskTypeIdEmpty_ThrowsDomainException()
        {
            // Arrange
            var title = "Title";
            var description = "desc";
            var taskTypeId = Guid.Empty;
            DateTime? due = null;

            // Act & Assert
            DomainException? ex1 = null;
            try
            {
                new TaskItem(title, description, taskTypeId, due);
            }
            catch (DomainException e)
            {
                ex1 = e;
            }
            Assert.IsNotNull(ex1);
            StringAssert.Contains(ex1!.Message, "Task type id is required.");
            StringAssert.Contains(ex1.Message, nameof(taskTypeId));
        }

        [TestMethod]
        public void Constructor_TitleWhitespace_ThrowsDomainException()
        {
            // Arrange
            var title = "   ";
            var description = "desc";
            var taskTypeId = Guid.NewGuid();
            DateTime? due = null;

            // Act & Assert
            DomainException? ex2 = null;
            try
            {
                new TaskItem(title, description, taskTypeId, due);
            }
            catch (DomainException e)
            {
                ex2 = e;
            }
            Assert.IsNotNull(ex2);
            StringAssert.Contains(ex2!.Message, "Title is required.");
            StringAssert.Contains(ex2.Message, "title");
        }

        [TestMethod]
        public void Constructor_DescriptionWhitespace_SetsDescriptionNull()
        {
            // Arrange
            var title = "Title";
            var description = "   ";
            var taskTypeId = Guid.NewGuid();
            DateTime? due = null;

            // Act
            var item = new TaskItem(title, description, taskTypeId, due);

            // Assert
            Assert.IsNull(item.Description);
        }

        [TestMethod]
        public void Update_ValidInputs_UpdatesProperties()
        {
            // Arrange
            var item = new TaskItem("Initial", "init desc", Guid.NewGuid(), null);
            var created = item.CreatedAtUtc;

            var newTitle = "  New Title  ";
            var newDescription = "  New Desc  ";
            var newTaskTypeId = Guid.NewGuid();
            var newStatus = TMTaskStatus.New; // set same or different
            var newDue = DateTime.UtcNow.AddDays(10);

            // Act
            item.Update(newTitle, newDescription, newTaskTypeId, newStatus, newDue);

            // Assert
            Assert.AreEqual("New Title", item.Title);
            Assert.AreEqual("New Desc", item.Description);
            Assert.AreEqual(newTaskTypeId, item.TaskTypeId);
            Assert.AreEqual(newStatus, item.Status);
            Assert.AreEqual(newDue, item.DueDateUtc);
            Assert.IsNotNull(item.UpdatedAtUtc);
            Assert.IsTrue(item.UpdatedAtUtc.Value >= created, "UpdatedAtUtc should be after or equal CreatedAtUtc");
        }

        [TestMethod]
        public void Update_TaskTypeIdEmpty_ThrowsDomainException()
        {
            // Arrange
            var item = new TaskItem("Title", null, Guid.NewGuid(), null);

            // Act & Assert
            DomainException? ex3 = null;
            try
            {
                item.Update("New", "desc", Guid.Empty, TMTaskStatus.New, null);
            }
            catch (DomainException e)
            {
                ex3 = e;
            }
            Assert.IsNotNull(ex3);
            StringAssert.Contains(ex3!.Message, "Task type id is required.");
            StringAssert.Contains(ex3.Message, nameof(Guid.Empty).Replace("Empty", ""));
        }

        [TestMethod]
        public void Update_TitleWhitespace_ThrowsDomainException()
        {
            // Arrange
            var item = new TaskItem("Title", null, Guid.NewGuid(), null);

            // Act & Assert
            DomainException? ex4 = null;
            try
            {
                item.Update("   ", "desc", Guid.NewGuid(), TMTaskStatus.New, null);
            }
            catch (DomainException e)
            {
                ex4 = e;
            }
            Assert.IsNotNull(ex4);
            StringAssert.Contains(ex4!.Message, "Title is required.");
            StringAssert.Contains(ex4.Message, "title");
        }

        [TestMethod]
        public void Update_DescriptionWhitespace_SetsDescriptionNull()
        {
            // Arrange
            var item = new TaskItem("Title", "desc", Guid.NewGuid(), null);

            // Act
            item.Update("New", "   ", Guid.NewGuid(), TMTaskStatus.New, null);

            // Assert
            Assert.IsNull(item.Description);
        }
    }
}
