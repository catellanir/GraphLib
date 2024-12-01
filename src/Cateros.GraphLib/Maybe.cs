public abstract class Maybe<T>
{
    public abstract Maybe<T1> Map<T1>(Func<T, T1> f);
    public abstract TResult MatchWith<TResult>((Func<TResult> None, Func<T, TResult> Some) pattern);
}
public class None<T> : Maybe<T>
{
    public None() { }
    public override TResult MatchWith<TResult>((Func<TResult> None, Func<T, TResult> Some) pattern) => pattern.None();
    public override Maybe<T1> Map<T1>(Func<T, T1> f) => new None<T1>();
}

public class Some<T> : Maybe<T>
{
    private readonly T value;
    public Some(T value) => this.value = value;
    public override TResult MatchWith<TResult>((Func<TResult> None, Func<T, TResult> Some) pattern) => pattern.Some(value);
    public override Maybe<T1> Map<T1>(Func<T, T1> f) => new Some<T1>(f(value));

}