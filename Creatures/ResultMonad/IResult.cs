using System;

namespace Creatures.ResultMonad
{
    public interface IResult<out T>
    {
        bool IsSucceded { get; }
        T Value { get; }
        string Error { get; }
        Result<TNew> Map<TNew>(Func<T, TNew> f);
    }
}