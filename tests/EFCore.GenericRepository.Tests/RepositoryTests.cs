using System.Data;
using Microsoft.EntityFrameworkCore;
using TanvirArjel.EFCore.GenericRepository;
using EFCore.GenericRepository.Tests.TestModels;

namespace EFCore.GenericRepository.Tests;

public class RepositoryTests
{
    private TestDbContext CreateTestDbContext(string databaseName = "TestDb")
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;

        return new TestDbContext(options);
    }

    private IRepository<TestDbContext> CreateRepository(TestDbContext dbContext)
    {
        return new Repository<TestDbContext>(dbContext);
    }

    #region BeginTransactionAsync Tests

    [Fact]
    public async Task BeginTransactionAsync_WithDefaultIsolationLevel_ShouldThrowDueToInMemoryLimitation()
    {
        var dbContext = CreateTestDbContext(Guid.NewGuid().ToString());
        var repository = CreateRepository(dbContext);

        try
        {
            // In-memory database doesn't support transactions, so this will throw
            await Assert.ThrowsAsync<InvalidOperationException>(() => repository.BeginTransactionAsync());
        }
        finally
        {
            await dbContext.DisposeAsync();
        }
    }

    [Fact]
    public async Task BeginTransactionAsync_WithSpecificIsolationLevel_ShouldThrowDueToInMemoryLimitation()
    {
        var dbContext = CreateTestDbContext(Guid.NewGuid().ToString());
        var repository = CreateRepository(dbContext);

        try
        {
            // In-memory database doesn't support transactions, so this will throw
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                repository.BeginTransactionAsync(IsolationLevel.ReadCommitted));
        }
        finally
        {
            await dbContext.DisposeAsync();
        }
    }

    [Fact]
    public async Task BeginTransactionAsync_WithCancellationToken_ShouldThrowDueToInMemoryLimitation()
    {
        var dbContext = CreateTestDbContext(Guid.NewGuid().ToString());
        var repository = CreateRepository(dbContext);

        try
        {
            // In-memory database doesn't support transactions, so this will throw
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                repository.BeginTransactionAsync(IsolationLevel.Unspecified, CancellationToken.None));
        }
        finally
        {
            await dbContext.DisposeAsync();
        }
    }

    #endregion

    #region Add Tests

    [Fact]
    public void Add_WithValidEntity_ShouldAddToChangeTracker()
    {
        var dbContext = CreateTestDbContext(Guid.NewGuid().ToString());
        var repository = CreateRepository(dbContext);
        var entity = new TestEntity { Name = "Test", Description = "Description", Value = 100, IsActive = true };

        try
        {
            repository.Add(entity);

            var entry = dbContext.ChangeTracker.Entries<TestEntity>().FirstOrDefault();
            Assert.NotNull(entry);
            Assert.Equal(EntityState.Added, entry.State);
        }
        finally
        {
            dbContext.Dispose();
        }
    }

    [Fact]
    public void Add_WithNullEntity_ShouldThrowArgumentNullException()
    {
        var dbContext = CreateTestDbContext(Guid.NewGuid().ToString());
        var repository = CreateRepository(dbContext);

        try
        {
            Assert.Throws<ArgumentNullException>(() => repository.Add<TestEntity>((TestEntity)null));
        }
        finally
        {
            dbContext.Dispose();
        }
    }

    [Fact]
    public void Add_WithMultipleValidEntities_ShouldAddAllToChangeTracker()
    {
        var dbContext = CreateTestDbContext(Guid.NewGuid().ToString());
        var repository = CreateRepository(dbContext);
        var entities = new List<TestEntity>
        {
            new TestEntity { Name = "Entity1", Description = "Description1", Value = 100, IsActive = true },
            new TestEntity { Name = "Entity2", Description = "Description2", Value = 200, IsActive = false }
        };

        try
        {
            // Add each entity individually since the overload expects a collection
            foreach (var entity in entities)
            {
                repository.Add(entity);
            }

            var entries = dbContext.ChangeTracker.Entries<TestEntity>().ToList();
            Assert.Equal(2, entries.Count);
            Assert.All(entries, e => Assert.Equal(EntityState.Added, e.State));
        }
        finally
        {
            dbContext.Dispose();
        }
    }

    [Fact]
    public void Add_WithNullEntitiesCollection_ShouldThrowArgumentNullException()
    {
        var dbContext = CreateTestDbContext(Guid.NewGuid().ToString());
        var repository = CreateRepository(dbContext);

        try
        {
            Assert.Throws<ArgumentNullException>(() => repository.Add<TestEntity>((IEnumerable<TestEntity>)null));
        }
        finally
        {
            dbContext.Dispose();
        }
    }

    #endregion

    #region AddAsync Tests

    [Fact]
    public async Task AddAsync_WithValidEntity_ShouldAddToChangeTracker()
    {
        var dbContext = CreateTestDbContext(Guid.NewGuid().ToString());
        var repository = CreateRepository(dbContext);
        var entity = new TestEntity { Name = "Test", Description = "Description", Value = 100, IsActive = true };

        try
        {
            await repository.AddAsync(entity);

            var entry = dbContext.ChangeTracker.Entries<TestEntity>().FirstOrDefault();
            Assert.NotNull(entry);
            Assert.Equal(EntityState.Added, entry.State);
        }
        finally
        {
            await dbContext.DisposeAsync();
        }
    }

    [Fact]
    public async Task AddAsync_WithNullEntity_ShouldThrowArgumentNullException()
    {
        var dbContext = CreateTestDbContext(Guid.NewGuid().ToString());
        var repository = CreateRepository(dbContext);

        try
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => repository.AddAsync<TestEntity>((TestEntity)null));
        }
        finally
        {
            await dbContext.DisposeAsync();
        }
    }

    [Fact]
    public async Task AddAsync_WithMultipleValidEntities_ShouldAddAllToChangeTracker()
    {
        var dbContext = CreateTestDbContext(Guid.NewGuid().ToString());
        var repository = CreateRepository(dbContext);
        var entities = new List<TestEntity>
        {
            new TestEntity { Name = "Entity1", Description = "Description1", Value = 100, IsActive = true },
            new TestEntity { Name = "Entity2", Description = "Description2", Value = 200, IsActive = false }
        };

        try
        {
            // Add each entity individually for collection handling
            foreach (var entity in entities)
            {
                await repository.AddAsync(entity);
            }

            var entries = dbContext.ChangeTracker.Entries<TestEntity>().ToList();
            Assert.Equal(2, entries.Count);
            Assert.All(entries, e => Assert.Equal(EntityState.Added, e.State));
        }
        finally
        {
            await dbContext.DisposeAsync();
        }
    }

    [Fact]
    public async Task AddAsync_WithNullEntitiesCollection_ShouldThrowArgumentNullException()
    {
        var dbContext = CreateTestDbContext(Guid.NewGuid().ToString());
        var repository = CreateRepository(dbContext);

        try
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => repository.AddAsync<TestEntity>((IEnumerable<TestEntity>)null));
        }
        finally
        {
            await dbContext.DisposeAsync();
        }
    }

    #endregion

    #region Update Tests

    [Fact]
    public void Update_WithValidEntity_ShouldMarkAsModified()
    {
        var dbContext = CreateTestDbContext(Guid.NewGuid().ToString());
        var repository = CreateRepository(dbContext);
        var entity = new TestEntity { Name = "Test", Description = "Description", Value = 100, IsActive = true };
        dbContext.TestEntities.Add(entity);
        dbContext.SaveChanges();

        entity.Name = "Updated";

        try
        {
            repository.Update(entity);

            var entry = dbContext.ChangeTracker.Entries<TestEntity>().FirstOrDefault();
            Assert.NotNull(entry);
            Assert.Equal(EntityState.Modified, entry.State);
        }
        finally
        {
            dbContext.Dispose();
        }
    }

    [Fact]
    public void Update_WithNullEntity_ShouldThrowArgumentNullException()
    {
        var dbContext = CreateTestDbContext(Guid.NewGuid().ToString());
        var repository = CreateRepository(dbContext);

        try
        {
            Assert.Throws<ArgumentNullException>(() => repository.Update<TestEntity>((TestEntity)null));
        }
        finally
        {
            dbContext.Dispose();
        }
    }

    [Fact]
    public void Update_WithEntityHavingDefaultPrimaryKey_ShouldThrowInvalidOperationException()
    {
        var dbContext = CreateTestDbContext(Guid.NewGuid().ToString());
        var repository = CreateRepository(dbContext);
        var entity = new TestEntity { Id = 0, Name = "Test", Description = "Description", Value = 100, IsActive = true };

        try
        {
            Assert.Throws<InvalidOperationException>(() => repository.Update(entity));
        }
        finally
        {
            dbContext.Dispose();
        }
    }

    [Fact]
    public void Update_WithTrackedEntity_ShouldMarkAsModified()
    {
        var dbContext = CreateTestDbContext(Guid.NewGuid().ToString());
        var repository = CreateRepository(dbContext);
        var entity = new TestEntity { Name = "Original", Description = "Description", Value = 100, IsActive = true };
        dbContext.TestEntities.Add(entity);
        dbContext.SaveChanges();

        entity.Name = "Updated";

        try
        {
            repository.Update(entity);

            var entry = dbContext.ChangeTracker.Entries<TestEntity>().FirstOrDefault();
            Assert.Equal(EntityState.Modified, entry.State);
        }
        finally
        {
            dbContext.Dispose();
        }
    }

    [Fact]
    public void Update_WithMultipleValidEntities_ShouldMarkAllAsModified()
    {
        var dbContext = CreateTestDbContext(Guid.NewGuid().ToString());
        var repository = CreateRepository(dbContext);
        var entities = new List<TestEntity>
        {
            new TestEntity { Name = "Entity1", Description = "Description1", Value = 100, IsActive = true },
            new TestEntity { Name = "Entity2", Description = "Description2", Value = 200, IsActive = false }
        };
        dbContext.TestEntities.AddRange(entities);
        dbContext.SaveChanges();

        entities[0].Name = "Updated1";
        entities[1].Name = "Updated2";

        try
        {
            // Update each entity individually
            foreach (var entity in entities)
            {
                repository.Update(entity);
            }

            var entries = dbContext.ChangeTracker.Entries<TestEntity>().ToList();
            Assert.All(entries, e => Assert.Equal(EntityState.Modified, e.State));
        }
        finally
        {
            dbContext.Dispose();
        }
    }

    [Fact]
    public void Update_WithNullEntitiesCollection_ShouldThrowArgumentNullException()
    {
        var dbContext = CreateTestDbContext(Guid.NewGuid().ToString());
        var repository = CreateRepository(dbContext);

        try
        {
            Assert.Throws<ArgumentNullException>(() => repository.Update<TestEntity>((IEnumerable<TestEntity>)null));
        }
        finally
        {
            dbContext.Dispose();
        }
    }

    #endregion

    #region Remove Tests

    [Fact]
    public void Remove_WithValidEntity_ShouldMarkAsDeleted()
    {
        var dbContext = CreateTestDbContext(Guid.NewGuid().ToString());
        var repository = CreateRepository(dbContext);
        var entity = new TestEntity { Name = "ToDelete", Description = "Description", Value = 100, IsActive = true };
        dbContext.TestEntities.Add(entity);
        dbContext.SaveChanges();

        try
        {
            repository.Remove(entity);

            var entry = dbContext.ChangeTracker.Entries<TestEntity>().FirstOrDefault();
            Assert.NotNull(entry);
            Assert.Equal(EntityState.Deleted, entry.State);
        }
        finally
        {
            dbContext.Dispose();
        }
    }

    [Fact]
    public void Remove_WithNullEntity_ShouldThrowArgumentNullException()
    {
        var dbContext = CreateTestDbContext(Guid.NewGuid().ToString());
        var repository = CreateRepository(dbContext);

        try
        {
            Assert.Throws<ArgumentNullException>(() => repository.Remove<TestEntity>((TestEntity)null));
        }
        finally
        {
            dbContext.Dispose();
        }
    }

    [Fact]
    public void Remove_WithMultipleValidEntities_ShouldMarkAllAsDeleted()
    {
        var dbContext = CreateTestDbContext(Guid.NewGuid().ToString());
        var repository = CreateRepository(dbContext);
        var entities = new List<TestEntity>
        {
            new TestEntity { Name = "Entity1", Description = "Description1", Value = 100, IsActive = true },
            new TestEntity { Name = "Entity2", Description = "Description2", Value = 200, IsActive = false }
        };
        dbContext.TestEntities.AddRange(entities);
        dbContext.SaveChanges();

        try
        {
            // Remove each entity individually
            foreach (var entity in entities)
            {
                repository.Remove(entity);
            }

            var entries = dbContext.ChangeTracker.Entries<TestEntity>().ToList();
            Assert.All(entries, e => Assert.Equal(EntityState.Deleted, e.State));
        }
        finally
        {
            dbContext.Dispose();
        }
    }

    [Fact]
    public void Remove_WithNullEntitiesCollection_ShouldThrowArgumentNullException()
    {
        var dbContext = CreateTestDbContext(Guid.NewGuid().ToString());
        var repository = CreateRepository(dbContext);

        try
        {
            Assert.Throws<ArgumentNullException>(() => repository.Remove<TestEntity>((IEnumerable<TestEntity>)null));
        }
        finally
        {
            dbContext.Dispose();
        }
    }

    #endregion

    #region SaveChangesAsync Tests

    [Fact]
    public async Task SaveChangesAsync_WithPendingChanges_ShouldPersistAndReturnRowCount()
    {
        var dbContext = CreateTestDbContext(Guid.NewGuid().ToString());
        var repository = CreateRepository(dbContext);
        var entity = new TestEntity { Name = "Test", Description = "Description", Value = 100, IsActive = true };
        repository.Add(entity);

        try
        {
            var result = await repository.SaveChangesAsync(CancellationToken.None);

            Assert.Equal(1, result);
            var count = await dbContext.TestEntities.CountAsync();
            Assert.Equal(1, count);
        }
        finally
        {
            await dbContext.DisposeAsync();
        }
    }

    [Fact]
    public async Task SaveChangesAsync_WithNoChanges_ShouldReturnZero()
    {
        var dbContext = CreateTestDbContext(Guid.NewGuid().ToString());
        var repository = CreateRepository(dbContext);

        try
        {
            var result = await repository.SaveChangesAsync(CancellationToken.None);

            Assert.Equal(0, result);
        }
        finally
        {
            await dbContext.DisposeAsync();
        }
    }

    [Fact]
    public async Task SaveChangesAsync_WithMultiplePendingChanges_ShouldPersistAll()
    {
        var dbContext = CreateTestDbContext(Guid.NewGuid().ToString());
        var repository = CreateRepository(dbContext);
        var entities = new List<TestEntity>
        {
            new TestEntity { Name = "Entity1", Description = "Description1", Value = 100, IsActive = true },
            new TestEntity { Name = "Entity2", Description = "Description2", Value = 200, IsActive = false }
        };
        // Add each entity individually
        foreach (var entity in entities)
        {
            await repository.AddAsync(entity);
        }

        try
        {
            var result = await repository.SaveChangesAsync(CancellationToken.None);

            Assert.Equal(2, result);
            var count = await dbContext.TestEntities.CountAsync();
            Assert.Equal(2, count);
        }
        finally
        {
            await dbContext.DisposeAsync();
        }
    }

    #endregion

    #region ClearChangeTracker Tests

    [Fact]
    public void ClearChangeTracker_WithTrackedEntities_ShouldClearAll()
    {
        var dbContext = CreateTestDbContext(Guid.NewGuid().ToString());
        var repository = CreateRepository(dbContext);
        var entity = new TestEntity { Name = "Test", Description = "Description", Value = 100, IsActive = true };
        dbContext.TestEntities.Add(entity);

        try
        {
            var trackedBefore = dbContext.ChangeTracker.Entries().Count();
            Assert.True(trackedBefore > 0);

            repository.ClearChangeTracker();

            var trackedAfter = dbContext.ChangeTracker.Entries().Count();
            Assert.Equal(0, trackedAfter);
        }
        finally
        {
            dbContext.Dispose();
        }
    }

    #endregion

    #region ExecuteDeleteAsync Tests

    [Fact]
    public async Task ExecuteDeleteAsync_WithoutCondition_ShouldThrowDueToInMemoryLimitation()
    {
        var dbContext = CreateTestDbContext(Guid.NewGuid().ToString());
        var repository = CreateRepository(dbContext);
        var entities = new List<TestEntity>
        {
            new TestEntity { Name = "Entity1", Description = "Description1", Value = 100, IsActive = true },
            new TestEntity { Name = "Entity2", Description = "Description2", Value = 200, IsActive = false }
        };
        dbContext.TestEntities.AddRange(entities);
        await dbContext.SaveChangesAsync();

        try
        {
            // In-memory database doesn't support ExecuteDelete
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                repository.ExecuteDeleteAsync<TestEntity>());
        }
        finally
        {
            await dbContext.DisposeAsync();
        }
    }

    [Fact]
    public async Task ExecuteDeleteAsync_WithCondition_ShouldThrowDueToInMemoryLimitation()
    {
        var dbContext = CreateTestDbContext(Guid.NewGuid().ToString());
        var repository = CreateRepository(dbContext);
        var entities = new List<TestEntity>
        {
            new TestEntity { Name = "Active1", Description = "Description1", Value = 100, IsActive = true },
            new TestEntity { Name = "Inactive1", Description = "Description2", Value = 200, IsActive = false },
            new TestEntity { Name = "Active2", Description = "Description3", Value = 300, IsActive = true }
        };
        dbContext.TestEntities.AddRange(entities);
        await dbContext.SaveChangesAsync();

        try
        {
            // In-memory database doesn't support ExecuteDelete
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                repository.ExecuteDeleteAsync<TestEntity>(e => !e.IsActive));
        }
        finally
        {
            await dbContext.DisposeAsync();
        }
    }

    [Fact]
    public async Task ExecuteDeleteAsync_WithConditionMatchingNone_ShouldThrowDueToInMemoryLimitation()
    {
        var dbContext = CreateTestDbContext(Guid.NewGuid().ToString());
        var repository = CreateRepository(dbContext);
        var entities = new List<TestEntity>
        {
            new TestEntity { Name = "Entity1", Description = "Description1", Value = 100, IsActive = true },
            new TestEntity { Name = "Entity2", Description = "Description2", Value = 200, IsActive = false }
        };
        dbContext.TestEntities.AddRange(entities);
        await dbContext.SaveChangesAsync();

        try
        {
            // In-memory database doesn't support ExecuteDelete
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                repository.ExecuteDeleteAsync<TestEntity>(e => e.Value > 1000));
        }
        finally
        {
            await dbContext.DisposeAsync();
        }
    }

    #endregion

    #region Integration Tests

    [Fact]
    public async Task FullWorkflow_AddUpdateDelete_ShouldWorkCorrectly()
    {
        var dbContext = CreateTestDbContext(Guid.NewGuid().ToString());
        var repository = CreateRepository(dbContext);
        var entity = new TestEntity { Name = "Original", Description = "Description", Value = 100, IsActive = true };

        try
        {
            // Add
            repository.Add(entity);
            var addResult = await repository.SaveChangesAsync(CancellationToken.None);
            Assert.Equal(1, addResult);

            var verifyAdd = await dbContext.TestEntities.FirstOrDefaultAsync();
            Assert.NotNull(verifyAdd);
            Assert.Equal("Original", verifyAdd.Name);

            // Update
            entity.Name = "Updated";
            repository.Update(entity);
            var updateResult = await repository.SaveChangesAsync(CancellationToken.None);
            Assert.Equal(1, updateResult);

            var verifyUpdate = await dbContext.TestEntities.FirstOrDefaultAsync();
            Assert.Equal("Updated", verifyUpdate.Name);

            // Delete
            repository.Remove(entity);
            var deleteResult = await repository.SaveChangesAsync(CancellationToken.None);
            Assert.Equal(1, deleteResult);

            var verifyDelete = await dbContext.TestEntities.FirstOrDefaultAsync();
            Assert.Null(verifyDelete);
        }
        finally
        {
            await dbContext.DisposeAsync();
        }
    }

    [Fact]
    public async Task BulkOperations_AddMultipleUpdateMultipleDelete_ShouldWorkCorrectly()
    {
        var dbContext = CreateTestDbContext(Guid.NewGuid().ToString());
        var repository = CreateRepository(dbContext);
        var entities = new List<TestEntity>
        {
            new TestEntity { Name = "Entity1", Description = "Description1", Value = 100, IsActive = true },
            new TestEntity { Name = "Entity2", Description = "Description2", Value = 200, IsActive = false },
            new TestEntity { Name = "Entity3", Description = "Description3", Value = 300, IsActive = true }
        };

        try
        {
            // Add Multiple - add each individually
            foreach (var entity in entities)
            {
                await repository.AddAsync(entity);
            }
            var addResult = await repository.SaveChangesAsync(CancellationToken.None);
            Assert.Equal(3, addResult);

            // Update Multiple
            var allEntities = await dbContext.TestEntities.ToListAsync();
            foreach (var entity in allEntities)
            {
                entity.Value += 100;
                repository.Update(entity);
            }
            var updateResult = await repository.SaveChangesAsync(CancellationToken.None);
            Assert.Equal(3, updateResult);

            // Delete Multiple
            var entitiesToDelete = await dbContext.TestEntities.ToListAsync();
            foreach (var entity in entitiesToDelete)
            {
                repository.Remove(entity);
            }
            var deleteResult = await repository.SaveChangesAsync(CancellationToken.None);
            Assert.Equal(3, deleteResult);

            var finalCount = await dbContext.TestEntities.CountAsync();
            Assert.Equal(0, finalCount);
        }
        finally
        {
            await dbContext.DisposeAsync();
        }
    }

    #endregion
}
