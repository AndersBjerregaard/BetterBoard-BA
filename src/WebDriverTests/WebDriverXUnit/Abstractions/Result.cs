namespace WebDriverXUnit.Abstractions;

public abstract class Result<T> {
    public class Ok(T value) : Result<T> {
        public T Value { get; } = value;
    }

    public class Err(Exception error) : Result<T> {
        public Exception Error { get; } = error;
    }

    public static Result<T> Success(T value) => new Ok(value);
    public static Result<T> Failure(Exception error) => new Err(error);

    public bool IsSuccess => this is Ok;
    public bool IsFailure => this is Err;

    public T GetValueOrThrow() {
        if (this is Ok ok) {
            return ok.Value;
        }
        throw new InvalidOperationException("Result does not contain a value");
    }

    public Exception GetErrorOrThrow() {
        if (this is Err err) {
            return err.Error;
        }
        throw new InvalidOperationException("Result does not contain an error");
    }
}