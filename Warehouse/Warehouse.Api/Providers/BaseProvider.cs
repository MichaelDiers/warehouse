namespace Warehouse.Api.Providers
{
    using MongoDB.Driver;
    using Warehouse.Api.Contracts.Database;
    using Warehouse.Api.Exceptions;
    using Warehouse.Api.Properties;

    /// <summary>
    ///     Base class for all providers.
    /// </summary>
    internal class BaseProvider
    {
        /// <summary>
        ///     The database transaction handler.
        /// </summary>
        private readonly ITransactionHandler transactionHandler;

        /// <summary>
        ///     The name of the child class.
        /// </summary>
        private readonly string typeName;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseProvider" /> class.
        /// </summary>
        protected BaseProvider(string typeName, ITransactionHandler transactionHandler)
        {
            this.typeName = typeName;
            this.transactionHandler = transactionHandler;
        }

        /// <summary>
        ///     Executes the specified database function handles its exceptions.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="T">The result of the database function <paramref name="func" />.</typeparam>
        /// <param name="func">The database function.</param>
        /// <param name="transform">
        ///     Transform the database result of type <typeparamref name="T" /> to type
        ///     <typeparamref name="TResult" />.
        /// </param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>The transformed result of <paramref name="func" />.</returns>
        /// <exception cref="Warehouse.Api.Exceptions.NotFoundException">Thrown if <paramref name="func" /> has no result.</exception>
        protected async Task<TResult> Execute<TResult, T>(
            Func<Task<IAsyncCursor<T>>> func,
            Func<T, TResult> transform,
            CancellationToken cancellationToken
        )
        {
            var result = await this.Execute(func);
            var entry = await result.FirstOrDefaultAsync(cancellationToken);
            if (entry is null)
            {
                throw new NotFoundException(this.Message(nameof(NotFoundException)));
            }

            return transform(entry);
        }

        /// <summary>
        ///     Executes the specified database function handles its exceptions.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="T">The result of the database function <paramref name="func" />.</typeparam>
        /// <param name="func">The database function.</param>
        /// <param name="transform">
        ///     Transform the database result of type <typeparamref name="T" /> to type
        ///     <typeparamref name="TResult" />.
        /// </param>
        /// <returns>The transformed result of <paramref name="func" />.</returns>
        /// <exception cref="Warehouse.Api.Exceptions.NotFoundException">Thrown if <paramref name="func" /> has no result.</exception>
        protected async Task<TResult> Execute<TResult, T>(Func<Task<T>> func, Func<T, TResult> transform)
            where T : class
        {
            var result = await this.Execute(func);
            if (result is null)
            {
                throw new NotFoundException(this.Message(nameof(NotFiniteNumberException)));
            }

            return transform(result);
        }

        /// <summary>
        ///     Executes the specified database delete function handles its exceptions.
        /// </summary>
        /// <param name="func">The database function.</param>
        /// <exception cref="Warehouse.Api.Exceptions.NotFoundException">Thrown if <paramref name="func" /> has no result.</exception>
        protected async Task Execute(Func<Task<DeleteResult>> func)
        {
            var result = await this.ExecuteGeneric(func);
            if (!result.IsAcknowledged || result.DeletedCount == 0)
            {
                throw new NotFoundException(this.Message(nameof(NotFoundException)));
            }
        }

        /// <summary>
        ///     Executes the specified database update function handles its exceptions.
        /// </summary>
        /// <param name="func">The database function.</param>
        /// <exception cref="Warehouse.Api.Exceptions.NotFoundException">Thrown if <paramref name="func" /> has no result.</exception>
        protected async Task Execute(Func<Task<UpdateResult>> func)
        {
            var result = await this.ExecuteGeneric(func);
            if (!result.IsAcknowledged || result.MatchedCount == 0)
            {
                throw new NotFoundException(this.Message(nameof(NotFoundException)));
            }
        }

        /// <summary>
        ///     Executes the specified database function.
        /// </summary>
        /// <typeparam name="T">The result of the database function <paramref name="func" />.</typeparam>
        /// <param name="func">The database function.</param>
        /// <returns>The result of <paramref name="func" />.</returns>
        /// <exception cref="ConflictException">Thrown if an entry in the database already exists.</exception>
        /// <exception cref="BadRequestException">Thrown if the database validation of the entry fails.</exception>
        protected async Task<T> Execute<T>(Func<Task<T>> func)
        {
            try
            {
                return await func();
            }
            catch (MongoWriteException e) when (e.WriteError.Code == 11000)
            {
                throw new ConflictException(this.Message(nameof(ConflictException)));
            }
            catch (Exception e) when ((e is MongoWriteException ex1 && ex1.WriteError.Code == 121) ||
                                      e is MongoCommandException {Code: 121})
            {
                throw new BadRequestException(this.Message(nameof(BadRequestException)));
            }
        }

        /// <summary>
        ///     Executes the specified database function.
        /// </summary>
        /// <param name="func">The database function.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        /// <exception cref="ConflictException">Thrown if an entry in the database already exists.</exception>
        /// <exception cref="BadRequestException">Thrown if the database validation of the entry fails.</exception>
        protected async Task Execute(Func<Task> func)
        {
            try
            {
                await func();
            }
            catch (MongoWriteException e) when (e.WriteError.Code == 11000)
            {
                throw new ConflictException(this.Message(nameof(ConflictException)));
            }
            catch (Exception e) when ((e is MongoWriteException ex1 && ex1.WriteError.Code == 121) ||
                                      e is MongoCommandException {Code: 121})
            {
                throw new BadRequestException(this.Message(nameof(BadRequestException)));
            }
        }

        /// <summary>
        ///     Executes the specified database function using a new transaction.
        /// </summary>
        /// <param name="func">The database function.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        protected async Task Execute(Func<ITransactionHandle, Task> func, CancellationToken cancellationToken)
        {
            using var session = await this.transactionHandler.StartTransactionAsync(cancellationToken);
            try
            {
                await this.Execute(() => func(session));
                await session.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await session.AbortTransactionAsync(cancellationToken);
                throw;
            }
        }

        /// <summary>
        ///     Executes the specified database function using a new database transaction.
        /// </summary>
        /// <typeparam name="T">The result of function <paramref name="func" />.</typeparam>
        /// <param name="func">The database function.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>The result of function <paramref name="func" />.</returns>
        protected async Task<T> Execute<T>(Func<ITransactionHandle, Task<T>> func, CancellationToken cancellationToken)
        {
            using var session = await this.transactionHandler.StartTransactionAsync(cancellationToken);
            try
            {
                var result = await this.Execute(() => func(session));
                await session.CommitTransactionAsync(cancellationToken);
                return result;
            }
            catch
            {
                await session.AbortTransactionAsync(cancellationToken);
                throw;
            }
        }

        /// <summary>
        ///     Executes the given database function.
        /// </summary>
        /// <typeparam name="T">The result type of the function <paramref name="func" />.</typeparam>
        /// <param name="func">The database function.</param>
        /// <returns>The result of function <paramref name="func" />.</returns>
        private Task<T> ExecuteGeneric<T>(Func<Task<T>> func)
        {
            return this.Execute(func);
        }

        /// <summary>
        ///     Read a text from the resources. The resource name is the combination of
        ///     the <see cref="typeName" /> and the given <paramref name="name" />.
        /// </summary>
        /// <param name="name">The second part of the resource name.</param>
        /// <returns>The errors message or string empty if not exists.</returns>
        private string Message(string name)
        {
            var message = Resources.ResourceManager.GetString($"{this.typeName}_{name}");
            return message ?? string.Empty;
        }
    }
}
