namespace Nonlinear_Solvers;

public struct Result<T>
{
    public EvalStatus Status;
    public int Iterations;
    public T Value;

    public Result(EvalStatus status, int iterations, T value)
    {
        Status = status;
        Iterations = iterations;
        Value = value;
    }
}