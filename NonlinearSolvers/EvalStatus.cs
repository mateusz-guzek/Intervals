namespace Nonlinear_Solvers;


[Flags]
public enum EvalStatus {
    ERROR,
    NOT_ACCURATE,
    FULL_SUCCESS,
    NO_SIGN_CHANGE,
    NO_CONVERGENCE,
    DIVISION_BY_ZERO
}