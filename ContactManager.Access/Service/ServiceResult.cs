using System;

namespace ContactManager.Access.Service
{
    /// <summary>
    /// Represents the result of a service operation with a specific data type.
    /// </summary>
    /// <typeparam name="T">The type of the result data.</typeparam>
    public class ServiceResult<T>
    {
        /// <summary>
        /// Gets or sets a value indicating whether the operation was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the result data.
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Gets or sets the message associated with the result.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Creates a successful result with the specified data.
        /// </summary>
        /// <param name="data">The result data.</param>
        /// <returns>A successful ServiceResult instance with the provided data.</returns>
        public static ServiceResult<T> SuccessResult(T data)
        {
            return new ServiceResult<T> { Success = true, Data = data };
        }

        /// <summary>
        /// Creates a failure result with the specified error message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>A failure ServiceResult instance with the provided error message.</returns>
        public static ServiceResult<T> FailureResult(string errorMessage)
        {
            return new ServiceResult<T> { Success = false, Message = errorMessage };
        }

        /// <summary>
        /// Creates a failure result with the specified error message and data.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="data">The result data.</param>
        /// <returns>A failure ServiceResult instance with the provided error message and data.</returns>
        public static ServiceResult<T> FailureResult(string errorMessage, T data)
        {
            return new ServiceResult<T> { Success = false, Data = data, Message = errorMessage };
        }
    }

    /// <summary>
    /// Represents the result of a service operation without specific result data.
    /// </summary>
    public class ServiceResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the operation was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the message associated with the result.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Creates a successful result.
        /// </summary>
        /// <returns>A successful ServiceResult instance.</returns>
        public static ServiceResult SuccessResult()
        {
            return new ServiceResult { Success = true };
        }

        /// <summary>
        /// Creates a failure result with the specified error message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>A failure ServiceResult instance with the provided error message.</returns>
        public static ServiceResult FailureResult(string errorMessage)
        {
            return new ServiceResult { Success = false, Message = errorMessage };
        }
    }
}
