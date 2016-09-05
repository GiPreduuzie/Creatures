using System;

namespace Creatures.ResultMonad
{
    public class Result<T> : IResult<T>
    {
        private Result(bool isSucceded, T value, string error)
        {
            IsSucceded = isSucceded;
            Value = value;
            Error = error;
        }

        public static Result<T> CreateFailure(string error) { return new Result<T>(false, default(T), error);}
        public static Result<T> CreateSuccess(T value) { return new Result<T>(true, value, ""); }

        public bool IsSucceded { get; }
        public T Value { get; }
        public string Error { get; }

        public Result<TNew> Map<TNew>(Func<T, TNew> f)
        {
            return IsSucceded ? Result<TNew>.CreateSuccess(f(Value)) : Result<TNew>.CreateFailure(Error);
        }
    }
}