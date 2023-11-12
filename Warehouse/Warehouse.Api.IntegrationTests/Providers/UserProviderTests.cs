namespace Warehouse.Api.IntegrationTests.Providers
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Warehouse.Api.Contracts.Database;
    using Warehouse.Api.Contracts.Users;
    using Warehouse.Api.Exceptions;
    using Warehouse.Api.Extensions;
    using Warehouse.Api.IntegrationTests.Lib;
    using Warehouse.Api.Models.Users;

    public class UserProviderTests
    {
        [Fact]
        public async Task CreateAsyncConflict()
        {
            var services = await UserProviderTests.Init();

            using var transactionHandle =
                await services.transactionHandler.StartTransactionAsync(new CancellationToken());
            await services.provider.CreateAsync(
                services.user,
                new CancellationToken(),
                transactionHandle);

            await Assert.ThrowsAsync<ConflictException>(
                () => services.provider.CreateAsync(
                    services.user,
                    new CancellationToken(),
                    transactionHandle));

            await transactionHandle.AbortTransactionAsync(new CancellationToken());
        }

        [Fact]
        public async Task CreateAsyncInvalidApplicationId()
        {
            var services = await UserProviderTests.Init();

            using var transactionHandle =
                await services.transactionHandler.StartTransactionAsync(new CancellationToken());
            await Assert.ThrowsAsync<BadRequestException>(
                () => services.provider.CreateAsync(
                    new User(
                        "x",
                        Guid.NewGuid().ToString(),
                        new[] {Role.Admin}),
                    new CancellationToken(),
                    transactionHandle));

            await transactionHandle.AbortTransactionAsync(new CancellationToken());
        }

        [Fact]
        public async Task CreateAsyncInvalidRoles()
        {
            var services = await UserProviderTests.Init();

            using var transactionHandle =
                await services.transactionHandler.StartTransactionAsync(new CancellationToken());
            await Assert.ThrowsAsync<BadRequestException>(
                () => services.provider.CreateAsync(
                    new User(
                        Guid.NewGuid().ToString(),
                        Guid.NewGuid().ToString(),
                        new[] {(Role) int.MaxValue}),
                    new CancellationToken(),
                    transactionHandle));

            await transactionHandle.AbortTransactionAsync(new CancellationToken());
        }

        [Fact]
        public async Task CreateAsyncOk()
        {
            var services = await UserProviderTests.Init();

            using var transactionHandle =
                await services.transactionHandler.StartTransactionAsync(new CancellationToken());
            await services.provider.CreateAsync(
                services.user,
                new CancellationToken(),
                transactionHandle);
            await transactionHandle.CommitTransactionAsync(new CancellationToken());
        }

        [Fact]
        public async Task DeleteAsyncNoSessionNotFound()
        {
            var services = await UserProviderTests.Init(true);

            await Assert.ThrowsAsync<NotFoundException>(
                () => services.provider.DeleteAsync(
                    Guid.NewGuid().ToString(),
                    new CancellationToken()));
        }

        [Fact]
        public async Task DeleteAsyncNoSessionOk()
        {
            var services = await UserProviderTests.Init(true);

            await services.provider.DeleteAsync(
                services.user.Id,
                new CancellationToken());
        }

        [Fact]
        public async Task DeleteAsyncNoSessionToLowerOk()
        {
            var services = await UserProviderTests.Init(true);

            await services.provider.DeleteAsync(
                services.user.Id.ToLower(),
                new CancellationToken());
        }

        [Fact]
        public async Task DeleteAsyncNoSessionToUpperOk()
        {
            var services = await UserProviderTests.Init(true);

            await services.provider.DeleteAsync(
                services.user.Id.ToUpper(),
                new CancellationToken());
        }

        [Fact]
        public async Task DeleteAsyncSessionNotFound()
        {
            var services = await UserProviderTests.Init(true);

            using var session = await services.transactionHandler.StartTransactionAsync(new CancellationToken());
            await Assert.ThrowsAsync<NotFoundException>(
                () => services.provider.DeleteAsync(
                    Guid.NewGuid().ToString(),
                    new CancellationToken(),
                    session));

            await session.AbortTransactionAsync(new CancellationToken());
        }

        [Fact]
        public async Task DeleteAsyncSessionOk()
        {
            var services = await UserProviderTests.Init(true);

            using var session = await services.transactionHandler.StartTransactionAsync(new CancellationToken());
            await services.provider.DeleteAsync(
                services.user.Id,
                new CancellationToken(),
                session);
            await session.CommitTransactionAsync(new CancellationToken());
        }

        [Fact]
        public async Task DeleteAsyncSessionToLowerOk()
        {
            var services = await UserProviderTests.Init(true);

            using var session = await services.transactionHandler.StartTransactionAsync(new CancellationToken());
            await services.provider.DeleteAsync(
                services.user.Id.ToLower(),
                new CancellationToken(),
                session);
            await session.CommitTransactionAsync(new CancellationToken());
        }

        [Fact]
        public async Task DeleteAsyncSessionToUpperOk()
        {
            var services = await UserProviderTests.Init(true);

            using var session = await services.transactionHandler.StartTransactionAsync(new CancellationToken());
            await services.provider.DeleteAsync(
                services.user.Id.ToUpper(),
                new CancellationToken(),
                session);
            await session.CommitTransactionAsync(new CancellationToken());
        }

        [Fact]
        public async Task ReadAsyncNoSessionOk()
        {
            var services = await UserProviderTests.Init(true);

            var result = await services.provider.ReadAsync(new CancellationToken());

            Assert.Contains(
                result,
                entry => entry.Id == services.user.Id && entry.Password == services.user.Password);
        }

        [Fact]
        public async Task ReadAsyncSessionOk()
        {
            var services = await UserProviderTests.Init(true);

            var session = await services.transactionHandler.StartTransactionAsync(new CancellationToken());
            var result = await services.provider.ReadAsync(
                new CancellationToken(),
                session);
            await session.CommitTransactionAsync(new CancellationToken());

            Assert.Contains(
                result,
                entry => entry.Id == services.user.Id && entry.Password == services.user.Password);
        }

        [Fact]
        public async Task ReadByIdAsyncNoSessionLowerOk()
        {
            var services = await UserProviderTests.Init(true);

            var result = await services.provider.ReadByIdAsync(
                services.user.Id.ToLower(),
                new CancellationToken());

            Assert.Equal(
                result.Id,
                services.user.Id);
            Assert.Equal(
                result.Password,
                services.user.Password);
            foreach (var resultRole in result.Roles)
            {
                Assert.Contains(
                    services.user.Roles,
                    role => role == resultRole);
            }
        }

        [Fact]
        public async Task ReadByIdAsyncNoSessionNotFound()
        {
            var services = await UserProviderTests.Init(true);

            await Assert.ThrowsAsync<NotFoundException>(
                () => services.provider.ReadByIdAsync(
                    Guid.NewGuid().ToString(),
                    new CancellationToken()));
        }

        [Fact]
        public async Task ReadByIdAsyncNoSessionOk()
        {
            var services = await UserProviderTests.Init(true);

            var result = await services.provider.ReadByIdAsync(
                services.user.Id,
                new CancellationToken());

            Assert.Equal(
                result.Id,
                services.user.Id);
            Assert.Equal(
                result.Password,
                services.user.Password);
            foreach (var resultRole in result.Roles)
            {
                Assert.Contains(
                    services.user.Roles,
                    role => role == resultRole);
            }
        }

        [Fact]
        public async Task ReadByIdAsyncNoSessionUpperOk()
        {
            var services = await UserProviderTests.Init(true);

            var result = await services.provider.ReadByIdAsync(
                services.user.Id.ToUpper(),
                new CancellationToken());

            Assert.Equal(
                result.Id,
                services.user.Id);
            Assert.Equal(
                result.Password,
                services.user.Password);
            foreach (var resultRole in result.Roles)
            {
                Assert.Contains(
                    services.user.Roles,
                    role => role == resultRole);
            }
        }

        [Fact]
        public async Task ReadByIdAsyncSessionNotFound()
        {
            var services = await UserProviderTests.Init(true);

            using var session = await services.transactionHandler.StartTransactionAsync(new CancellationToken());
            await Assert.ThrowsAsync<NotFoundException>(
                () => services.provider.ReadByIdAsync(
                    Guid.NewGuid().ToString(),
                    new CancellationToken(),
                    session));

            await session.AbortTransactionAsync(new CancellationToken());
        }

        [Fact]
        public async Task ReadByIdAsyncSessionOk()
        {
            var services = await UserProviderTests.Init(true);

            using var session = await services.transactionHandler.StartTransactionAsync(new CancellationToken());
            var result = await services.provider.ReadByIdAsync(
                services.user.Id,
                new CancellationToken(),
                session);
            await session.CommitTransactionAsync(new CancellationToken());

            Assert.Equal(
                result.Id,
                services.user.Id);
            Assert.Equal(
                result.Password,
                services.user.Password);
            foreach (var resultRole in result.Roles)
            {
                Assert.Contains(
                    services.user.Roles,
                    role => role == resultRole);
            }
        }

        [Fact]
        public async Task UpdateAsyncNoSessionInvalidPassword()
        {
            var services = await UserProviderTests.Init(true);

            var update = new User(
                services.user.Id,
                new string(
                    'a',
                    101),
                new[] {Role.None});

            await Assert.ThrowsAsync<BadRequestException>(
                () => services.provider.UpdateAsync(
                    update,
                    new CancellationToken()));
        }

        [Fact]
        public async Task UpdateAsyncNoSessionInvalidRoles()
        {
            var services = await UserProviderTests.Init(true);

            var update = new User(
                services.user.Id,
                services.user.Password,
                new[] {(Role) int.MaxValue});

            await Assert.ThrowsAsync<BadRequestException>(
                () => services.provider.UpdateAsync(
                    update,
                    new CancellationToken()));
        }

        [Fact]
        public async Task UpdateAsyncNoSessionNotFound()
        {
            var services = await UserProviderTests.Init(true);

            var update = new User(
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                new[] {Role.None});

            await Assert.ThrowsAsync<NotFoundException>(
                () => services.provider.UpdateAsync(
                    update,
                    new CancellationToken()));
        }

        [Fact]
        public async Task UpdateAsyncNoSessionOk()
        {
            var services = await UserProviderTests.Init(true);

            var update = new User(
                services.user.Id,
                Guid.NewGuid().ToString(),
                new[] {Role.None});

            await services.provider.UpdateAsync(
                update,
                new CancellationToken());

            var updated = await services.provider.ReadByIdAsync(
                services.user.Id,
                new CancellationToken());

            Assert.Equal(
                update.Id,
                updated.Id);
            Assert.Equal(
                update.Password,
                updated.Password);
            foreach (var updateRole in update.Roles)
            {
                Assert.Contains(
                    updated.Roles,
                    role => role == updateRole);
            }
        }

        [Fact]
        public async Task UpdateAsyncNoSessionOkLower()
        {
            var services = await UserProviderTests.Init(true);

            var update = new User(
                services.user.Id.ToLower(),
                Guid.NewGuid().ToString(),
                new[] {Role.None});

            await services.provider.UpdateAsync(
                update,
                new CancellationToken());

            var updated = await services.provider.ReadByIdAsync(
                services.user.Id,
                new CancellationToken());

            Assert.Equal(
                services.user.Id,
                updated.Id);
            Assert.Equal(
                update.Password,
                updated.Password);
            foreach (var updateRole in update.Roles)
            {
                Assert.Contains(
                    updated.Roles,
                    role => role == updateRole);
            }
        }

        [Fact]
        public async Task UpdateAsyncNoSessionOkToUpper()
        {
            var services = await UserProviderTests.Init(true);

            var update = new User(
                services.user.Id.ToUpper(),
                Guid.NewGuid().ToString(),
                new[] {Role.None});

            await services.provider.UpdateAsync(
                update,
                new CancellationToken());

            var updated = await services.provider.ReadByIdAsync(
                services.user.Id,
                new CancellationToken());

            Assert.Equal(
                services.user.Id,
                updated.Id);
            Assert.Equal(
                update.Password,
                updated.Password);
            foreach (var updateRole in update.Roles)
            {
                Assert.Contains(
                    updated.Roles,
                    role => role == updateRole);
            }
        }

        [Fact]
        public async Task UpdateAsyncSessionInvalidPassword()
        {
            var services = await UserProviderTests.Init(true);

            var session = await services.transactionHandler.StartTransactionAsync(new CancellationToken());
            var update = new User(
                services.user.Id,
                new string(
                    'a',
                    101),
                new[] {Role.User});

            await Assert.ThrowsAsync<BadRequestException>(
                () => services.provider.UpdateAsync(
                    update,
                    new CancellationToken(),
                    session));

            await session.AbortTransactionAsync(new CancellationToken());
        }

        [Fact]
        public async Task UpdateAsyncSessionInvalidRoles()
        {
            var services = await UserProviderTests.Init(true);

            var session = await services.transactionHandler.StartTransactionAsync(new CancellationToken());
            var update = new User(
                services.user.Id,
                Guid.NewGuid().ToString(),
                new[] {(Role) int.MaxValue});

            await Assert.ThrowsAsync<BadRequestException>(
                () => services.provider.UpdateAsync(
                    update,
                    new CancellationToken(),
                    session));

            await session.AbortTransactionAsync(new CancellationToken());
        }

        [Fact]
        public async Task UpdateAsyncSessionNotFound()
        {
            var services = await UserProviderTests.Init(true);

            var session = await services.transactionHandler.StartTransactionAsync(new CancellationToken());
            var update = new User(
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                new[] {Role.None});

            await Assert.ThrowsAsync<NotFoundException>(
                () => services.provider.UpdateAsync(
                    update,
                    new CancellationToken(),
                    session));

            await session.AbortTransactionAsync(new CancellationToken());
        }

        [Fact]
        public async Task UpdateAsyncSessionOk()
        {
            var services = await UserProviderTests.Init(true);

            var session = await services.transactionHandler.StartTransactionAsync(new CancellationToken());
            var update = new User(
                services.user.Id,
                Guid.NewGuid().ToString(),
                new[] {Role.None});

            await services.provider.UpdateAsync(
                update,
                new CancellationToken(),
                session);
            await session.CommitTransactionAsync(new CancellationToken());

            var updated = await services.provider.ReadByIdAsync(
                services.user.Id,
                new CancellationToken());

            Assert.Equal(
                update.Id,
                updated.Id);
            Assert.Equal(
                update.Password,
                updated.Password);
            foreach (var updateRole in update.Roles)
            {
                Assert.Contains(
                    updated.Roles,
                    role => role == updateRole);
            }
        }

        private static async Task<(IUser user, IUserProvider provider, ITransactionHandler transactionHandler )> Init(
            bool createUser = false
        )
        {
            var builder = Host.CreateApplicationBuilder();
            TestMongoClient.Connect(builder.Services);

            builder.Services.AddDependencies();
            var app = builder.Build();

            var provider = app.Services.GetService<IUserProvider>();
            Assert.NotNull(provider);

            var transactionHandler = app.Services.GetService<ITransactionHandler>();
            Assert.NotNull(transactionHandler);

            var user = UserProviderTests.User();

            if (createUser)
            {
                using var transactionHandle = await transactionHandler.StartTransactionAsync(new CancellationToken());
                await provider.CreateAsync(
                    user,
                    new CancellationToken(),
                    transactionHandle);
                await transactionHandle.CommitTransactionAsync(new CancellationToken());
            }

            return (user, provider, transactionHandler);
        }

        private static IUser User()
        {
            return new User(
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                new[] {Role.User});
        }
    }
}
