# AdventOfCodeLibrary

## Simple AdventOfCode helper library

I created this library to assist in faster gathering and submission for problems from AdventOfCode.

### Usage

Use developer tools to get your session cookie from AOC. This will be used for retrieving your user specific input, and submitting your results.

![AdventOfCodeSession](https://github.com/NeonDactyl/AdventOfCodeLib/assets/42546733/0463be58-7a0d-47e4-885c-0f4b87e18a58)


If using inside a repository that will be pushed remotely, I recommend saving it in a file that is in your .gitignore, or saving it into environment variables to load into your application.

Create a class that inherits from `AdventOfCodeDay` for the problem on a given day, for example:
```csharp
public class Aoc15_1 : AdventOfCodeDay
{
    public Aoc15_1(string session = "") : base(2015, 1, session) { }
    public override string PartOne(string input)
    {
        return String.Empty;
    }
    public override string PartTwo(string input)
    {
        return String.Empty;
    }
}
```

This sets up the necessary framework to let your problem solving begin.

When testing your code, you can call the `PartOne` and `PartTwo` methods with a specific string directly. If no string is used, the parent class will use the input retrieved using the session cookie you provided. If an invalid session cookie is provided, a `System.Net.WebException` will be thrown. This will happen on initialization of the class instance.

Once your class is written, and you're ready to test, inside your program, you can create an instance of your class, specifying the session, and do any testing, as shown in this example:

```csharp
class Program
{
    static void Main()
    {
        var aoc15_1 = new Aoc15_1("SOME_SESSION_COOKIE_VALUE");
        var your_input = aoc15_1.Input;
        var testingInput = "5 9 2 8\n9 4 7 3\n3 8 6 5";
        var testresult2 = aoc15_1.PartTwo(testingInput);

        var result1 = aoc15_1.PartOne();
        var result2 = aoc15_1.PartTwo();

        int isCorrect1 = aoc15_1.SubmitPartOne(result1);
        int isCorrect2 = aoc15_1.SubmitPartTwo(result2);

        if (isCorrect2 == 0) Console.WriteLine("got it right");
        else if (isCorrect2 == 1) Console.WriteLine("Answer was too high");
        else if (isCorrect2 == -1) Console.WriteLine("Answer was too low");
        else throw new Exception("what happened here");

        Console.WriteLine(result2);
    }
}
```

One thing to note is that the SubmitPartOne and SubmitPartTwo methods return an int i by reading the response from the server:
- 0 if your result equals the expected (AKA you're correct)
- 1 if your result is too high
- -1 if your result is too low
