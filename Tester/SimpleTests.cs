using Functions;
using Nonlinear_Solvers;
using Numerics.NET;
using UserFunctions;
using Xunit.Abstractions;
using Interval = Intervals.Interval;

namespace Tester;

public class SimpleTests
{
    
    private readonly ITestOutputHelper output;

    public SimpleTests(ITestOutputHelper output)
    {
        this.output = output;
    }
    
    
    [Fact]
    public void FalsiInterval()
    {
        MyFunctions f = new MyFunctions();
        Result<Interval> result = RegulaFalsi.EvalI(f.Functions().ElementAt(1), -1.0, 1.0, 100, 1e-16);
        output.WriteLine(result.Status.ToString());
        //output.WriteLine("["+BigFloat.Sqrt(2).ToString() + "0000000000 , " + BigFloat.Sqrt(2).ToString());
        output.WriteLine(result.Value.ToString());
        output.WriteLine(result.Iterations.ToString());
        
        Assert.True(result.Value.Contains(0.317348));
    }
}