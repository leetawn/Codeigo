using Codeigo.Models;

namespace Codeigo.Services;

/// <summary>
/// Provides the full content catalogue: languages → tracks → lessons → exercises.
/// In v1 this is static in-memory data. Replace with JSON/DB loading later.
/// </summary>
public class ContentService
{
    private readonly List<LanguageCurriculum> _curricula;

    public ContentService()
    {
        _curricula = BuildCurricula();
    }

    public IReadOnlyList<LanguageCurriculum> Languages => _curricula.AsReadOnly();

    public LanguageCurriculum? GetLanguage(string id) =>
        _curricula.FirstOrDefault(l => l.Id == id);

    public Track? GetTrack(string langId, string trackId) =>
        GetLanguage(langId)?.Tracks.FirstOrDefault(t => t.Id == trackId);

    public Lesson? GetLesson(string langId, string trackId, string lessonId) =>
        GetTrack(langId, trackId)?.Lessons.FirstOrDefault(l => l.Id == lessonId);

    // ── Data ────────────────────────────────────────────────────────────────

    private static List<LanguageCurriculum> BuildCurricula() =>
    [
        BuildPython(),
        BuildJavaScript(),
        BuildCpp()
    ];

    // ── Python ───────────────────────────────────────────────────────────────

    private static LanguageCurriculum BuildPython() => new()
    {
        Id = "python",
        DisplayName = "Python",
        Emoji = "🐍",
        AccentClass = "py",
        Tracks =
        [
            new Track
            {
                Id = "basics",
                Title = "Basics",
                Lessons =
                [
                    new Lesson
                    {
                        Id = "py-basics-1",
                        Title = "Your first print",
                        Exercises =
                        [
                            new Exercise
                            {
                                Type = ExerciseType.FillBlank,
                                Prompt = "Complete the line to print 'Hello, world!'",
                                Code = "[[print]]('Hello, world!')",
                                AcceptedAnswers = ["print"],
                                Explanation = "print() is the built-in function for outputting text.",
                                Language = "python", Track = "basics", Skill = "output", Difficulty = 1
                            },
                            new Exercise
                            {
                                Type = ExerciseType.MultipleChoice,
                                Prompt = "Which line prints the text 'score' to the screen?",
                                Options = ["echo 'score'", "print('score')", "console.log('score')", "puts 'score'"],
                                CorrectIndex = 1,
                                Explanation = "Python uses print(), not echo or console.log.",
                                Language = "python", Track = "basics", Skill = "output", Difficulty = 1
                            },
                            new Exercise
                            {
                                Type = ExerciseType.PredictOutput,
                                Prompt = "What does this print?",
                                Code = "print(2 + 3)",
                                ExpectedOutput = "5",
                                Explanation = "2 + 3 evaluates to 5, then print() shows it.",
                                Language = "python", Track = "basics", Skill = "output", Difficulty = 1
                            },
                            new Exercise
                            {
                                Type = ExerciseType.TypeItOut,
                                Prompt = "Type the line that prints 'Game over!'",
                                TargetLine = "print('Game over!')",
                                AcceptedAnswers = ["print('Game over!')", "print(\"Game over!\")"],
                                Explanation = "Either single or double quotes work in Python.",
                                Language = "python", Track = "basics", Skill = "output", Difficulty = 2
                            },
                            new Exercise
                            {
                                Type = ExerciseType.MultipleChoice,
                                Prompt = "What is the correct way to write a single-line comment in Python?",
                                Options = ["// This is a comment", "/* comment */", "# This is a comment", "-- comment"],
                                CorrectIndex = 2,
                                Explanation = "Python uses # for single-line comments.",
                                Language = "python", Track = "basics", Skill = "comments", Difficulty = 1
                            }
                        ]
                    },
                    new Lesson
                    {
                        Id = "py-basics-2",
                        Title = "Comments & style",
                        Exercises =
                        [
                            new Exercise
                            {
                                Type = ExerciseType.FillBlank,
                                Prompt = "Add a comment that says 'setup'",
                                Code = "[[#]] setup\nhealth = 100",
                                AcceptedAnswers = ["#"],
                                Explanation = "# starts a comment in Python.",
                                Language = "python", Track = "basics", Skill = "comments", Difficulty = 1
                            },
                            new Exercise
                            {
                                Type = ExerciseType.ArrangeCode,
                                Prompt = "Arrange the lines to print a greeting then a farewell",
                                Code = "print('Hello!')\nprint('Goodbye!')",
                                Language = "python", Track = "basics", Skill = "output", Difficulty = 2
                            },
                            new Exercise
                            {
                                Type = ExerciseType.MultipleChoice,
                                Prompt = "What does this print? print('5' + '5')",
                                Options = ["10", "55", "5 5", "Error"],
                                CorrectIndex = 1,
                                Explanation = "'5' + '5' concatenates strings, giving '55'.",
                                Language = "python", Track = "basics", Skill = "output", Difficulty = 2
                            }
                        ]
                    }
                ]
            },
            new Track
            {
                Id = "variables",
                Title = "Variables & Types",
                Lessons =
                [
                    new Lesson
                    {
                        Id = "py-vars-1",
                        Title = "Storing values",
                        Exercises =
                        [
                            new Exercise
                            {
                                Type = ExerciseType.FillBlank,
                                Prompt = "Store the player's health as 100",
                                Code = "health [[=]] 100",
                                AcceptedAnswers = ["="],
                                Explanation = "= is the assignment operator.",
                                Language = "python", Track = "variables", Skill = "assignment", Difficulty = 1
                            },
                            new Exercise
                            {
                                Type = ExerciseType.PredictOutput,
                                Prompt = "What is printed?",
                                Code = "name = 'Ethan'\nprint(name)",
                                ExpectedOutput = "Ethan",
                                Explanation = "name holds the string 'Ethan', print shows it without quotes.",
                                Language = "python", Track = "variables", Skill = "strings", Difficulty = 1
                            },
                            new Exercise
                            {
                                Type = ExerciseType.MultipleChoice,
                                Prompt = "Which of these is a valid Python variable name?",
                                Options = ["2score", "player-name", "enemy_count", "class"],
                                CorrectIndex = 2,
                                Explanation = "Variable names can't start with a digit, contain hyphens, or use reserved words like 'class'.",
                                Language = "python", Track = "variables", Skill = "naming", Difficulty = 2
                            },
                            new Exercise
                            {
                                Type = ExerciseType.PredictOutput,
                                Prompt = "What does this print?",
                                Code = "x = 10\nx = x + 5\nprint(x)",
                                ExpectedOutput = "15",
                                Explanation = "x starts at 10, then 5 is added, making it 15.",
                                Language = "python", Track = "variables", Skill = "assignment", Difficulty = 2
                            }
                        ]
                    }
                ]
            },
            new Track
            {
                Id = "loops",
                Title = "Loops",
                Lessons =
                [
                    new Lesson
                    {
                        Id = "py-loops-1",
                        Title = "for loops",
                        Exercises =
                        [
                            new Exercise
                            {
                                Type = ExerciseType.FillBlank,
                                Prompt = "Finish the loop so it runs 5 times",
                                Code = "for i in [[range]](5):\n    print(i)",
                                AcceptedAnswers = ["range"],
                                Explanation = "range(5) produces 0 to 4.",
                                Language = "python", Track = "loops", Skill = "for-loop", Difficulty = 1
                            },
                            new Exercise
                            {
                                Type = ExerciseType.PredictOutput,
                                Prompt = "What does this print?",
                                Code = "for i in range(3):\n    print(i)",
                                ExpectedOutput = "0\n1\n2",
                                Explanation = "range(3) gives 0, 1, 2. Each is printed on its own line.",
                                Language = "python", Track = "loops", Skill = "for-loop", Difficulty = 1
                            },
                            new Exercise
                            {
                                Type = ExerciseType.MultipleChoice,
                                Prompt = "How many times does this loop run? for i in range(2, 5):",
                                Options = ["2", "3", "4", "5"],
                                CorrectIndex = 1,
                                Explanation = "range(2, 5) gives 2, 3, 4 — three values.",
                                Language = "python", Track = "loops", Skill = "range", Difficulty = 2
                            },
                            new Exercise
                            {
                                Type = ExerciseType.ArrangeCode,
                                Prompt = "Arrange to loop over a list of enemies",
                                Code = "enemies = ['goblin', 'orc']\nfor enemy in enemies:\n    print(enemy)",
                                Language = "python", Track = "loops", Skill = "for-loop", Difficulty = 2
                            },
                            new Exercise
                            {
                                Type = ExerciseType.MultipleChoice,
                                Prompt = "Which keyword creates an infinite loop?",
                                Options = ["for", "while True:", "loop", "repeat"],
                                CorrectIndex = 1,
                                Explanation = "while True: loops forever until a break statement.",
                                Language = "python", Track = "loops", Skill = "while", Difficulty = 2
                            }
                        ]
                    },
                    new Lesson
                    {
                        Id = "py-loops-2",
                        Title = "while loops",
                        Exercises =
                        [
                            new Exercise
                            {
                                Type = ExerciseType.FillBlank,
                                Prompt = "Fill the keyword to keep looping while health > 0",
                                Code = "[[while]] health > 0:\n    take_damage()",
                                AcceptedAnswers = ["while"],
                                Explanation = "while repeats as long as its condition is True.",
                                Language = "python", Track = "loops", Skill = "while", Difficulty = 1
                            },
                            new Exercise
                            {
                                Type = ExerciseType.PredictOutput,
                                Prompt = "What is printed?",
                                Code = "x = 0\nwhile x < 3:\n    print(x)\n    x += 1",
                                ExpectedOutput = "0\n1\n2",
                                Explanation = "x starts at 0 and increments until it reaches 3.",
                                Language = "python", Track = "loops", Skill = "while", Difficulty = 2
                            }
                        ]
                    }
                ]
            },
            new Track
            {
                Id = "conditionals",
                Title = "Conditionals",
                Lessons =
                [
                    new Lesson
                    {
                        Id = "py-cond-1",
                        Title = "if / elif / else",
                        Exercises =
                        [
                            new Exercise
                            {
                                Type = ExerciseType.FillBlank,
                                Prompt = "Complete the condition to check if health is zero",
                                Code = "if health [[==]] 0:\n    print('Game over')",
                                AcceptedAnswers = ["=="],
                                Explanation = "== checks equality. = would assign, not compare.",
                                Language = "python", Track = "conditionals", Skill = "if", Difficulty = 1
                            },
                            new Exercise
                            {
                                Type = ExerciseType.PredictOutput,
                                Prompt = "What prints?",
                                Code = "score = 80\nif score >= 90:\n    print('A')\nelif score >= 80:\n    print('B')\nelse:\n    print('C')",
                                ExpectedOutput = "B",
                                Explanation = "score is 80, so the elif branch matches.",
                                Language = "python", Track = "conditionals", Skill = "elif", Difficulty = 2
                            }
                        ]
                    }
                ]
            },
            new Track
            {
                Id = "functions",
                Title = "Functions",
                Lessons =
                [
                    new Lesson
                    {
                        Id = "py-func-1",
                        Title = "Defining functions",
                        Exercises =
                        [
                            new Exercise
                            {
                                Type = ExerciseType.FillBlank,
                                Prompt = "Complete the keyword to define a function",
                                Code = "[[def]] greet(name):\n    print('Hi', name)",
                                AcceptedAnswers = ["def"],
                                Explanation = "def is the keyword to define a function in Python.",
                                Language = "python", Track = "functions", Skill = "def", Difficulty = 1
                            },
                            new Exercise
                            {
                                Type = ExerciseType.PredictOutput,
                                Prompt = "What does this print?",
                                Code = "def add(a, b):\n    return a + b\nprint(add(3, 4))",
                                ExpectedOutput = "7",
                                Explanation = "add(3, 4) returns 7, which print() displays.",
                                Language = "python", Track = "functions", Skill = "return", Difficulty = 2
                            }
                        ]
                    }
                ]
            }
        ]
    };

    // ── JavaScript ───────────────────────────────────────────────────────────

    private static LanguageCurriculum BuildJavaScript() => new()
    {
        Id = "javascript",
        DisplayName = "JavaScript",
        Emoji = "🟨",
        AccentClass = "js",
        Tracks =
        [
            new Track
            {
                Id = "basics",
                Title = "Basics",
                Lessons =
                [
                    new Lesson
                    {
                        Id = "js-basics-1",
                        Title = "console.log",
                        Exercises =
                        [
                            new Exercise
                            {
                                Type = ExerciseType.FillBlank,
                                Prompt = "Complete the line to print 'Hello!'",
                                Code = "[[console]].log('Hello!');",
                                AcceptedAnswers = ["console"],
                                Explanation = "JavaScript uses console.log() to print to the console.",
                                Language = "javascript", Track = "basics", Skill = "output", Difficulty = 1
                            },
                            new Exercise
                            {
                                Type = ExerciseType.PredictOutput,
                                Prompt = "What is logged?",
                                Code = "console.log(5 + '5');",
                                ExpectedOutput = "55",
                                Explanation = "5 + '5' coerces 5 to a string, giving '55'.",
                                Language = "javascript", Track = "basics", Skill = "coercion", Difficulty = 2
                            },
                            new Exercise
                            {
                                Type = ExerciseType.MultipleChoice,
                                Prompt = "Which is the correct single-line comment syntax in JavaScript?",
                                Options = ["# comment", "/* comment */", "// comment", "<!-- comment -->"],
                                CorrectIndex = 2,
                                Explanation = "// starts a single-line comment in JavaScript.",
                                Language = "javascript", Track = "basics", Skill = "comments", Difficulty = 1
                            }
                        ]
                    }
                ]
            },
            new Track
            {
                Id = "variables",
                Title = "Variables & Types",
                Lessons =
                [
                    new Lesson
                    {
                        Id = "js-vars-1",
                        Title = "let, const, var",
                        Exercises =
                        [
                            new Exercise
                            {
                                Type = ExerciseType.MultipleChoice,
                                Prompt = "Which keyword declares a variable that can be reassigned?",
                                Options = ["const", "let", "final", "static"],
                                CorrectIndex = 1,
                                Explanation = "let declares a block-scoped variable you can reassign. const cannot be reassigned.",
                                Language = "javascript", Track = "variables", Skill = "let-const", Difficulty = 1
                            },
                            new Exercise
                            {
                                Type = ExerciseType.FillBlank,
                                Prompt = "Declare a constant for max lives",
                                Code = "[[const]] MAX_LIVES = 3;",
                                AcceptedAnswers = ["const"],
                                Explanation = "const declares a constant. It cannot be reassigned.",
                                Language = "javascript", Track = "variables", Skill = "const", Difficulty = 1
                            },
                            new Exercise
                            {
                                Type = ExerciseType.PredictOutput,
                                Prompt = "What is logged?",
                                Code = "let x = 10;\nx += 5;\nconsole.log(x);",
                                ExpectedOutput = "15",
                                Explanation = "x starts at 10, += 5 makes it 15.",
                                Language = "javascript", Track = "variables", Skill = "assignment", Difficulty = 1
                            }
                        ]
                    }
                ]
            },
            new Track
            {
                Id = "loops",
                Title = "Loops",
                Lessons =
                [
                    new Lesson
                    {
                        Id = "js-loops-1",
                        Title = "for loops",
                        Exercises =
                        [
                            new Exercise
                            {
                                Type = ExerciseType.FillBlank,
                                Prompt = "Fill in the loop header to count 0 to 4",
                                Code = "for ([[let i = 0; i < 5; i++]]) {\n    console.log(i);\n}",
                                AcceptedAnswers = ["let i = 0; i < 5; i++", "let i=0; i<5; i++"],
                                Explanation = "A for loop has initializer; condition; incrementer.",
                                Language = "javascript", Track = "loops", Skill = "for", Difficulty = 2
                            },
                            new Exercise
                            {
                                Type = ExerciseType.PredictOutput,
                                Prompt = "What is logged?",
                                Code = "const items = ['sword', 'shield'];\nfor (const item of items) {\n    console.log(item);\n}",
                                ExpectedOutput = "sword\nshield",
                                Explanation = "for...of iterates over array values.",
                                Language = "javascript", Track = "loops", Skill = "for-of", Difficulty = 2
                            }
                        ]
                    }
                ]
            },
            new Track
            {
                Id = "functions",
                Title = "Functions",
                Lessons =
                [
                    new Lesson
                    {
                        Id = "js-func-1",
                        Title = "Arrow functions",
                        Exercises =
                        [
                            new Exercise
                            {
                                Type = ExerciseType.MultipleChoice,
                                Prompt = "Which is the arrow function syntax?",
                                Options = ["function add(a,b) { return a+b; }", "add = lambda a,b: a+b", "const add = (a, b) => a + b;", "def add(a, b): return a + b"],
                                CorrectIndex = 2,
                                Explanation = "Arrow functions use => in JavaScript.",
                                Language = "javascript", Track = "functions", Skill = "arrow", Difficulty = 2
                            },
                            new Exercise
                            {
                                Type = ExerciseType.FillBlank,
                                Prompt = "Complete the arrow function",
                                Code = "const double = (n) [[=>]] n * 2;",
                                AcceptedAnswers = ["=>"],
                                Explanation = "=> separates parameters from the function body.",
                                Language = "javascript", Track = "functions", Skill = "arrow", Difficulty = 1
                            }
                        ]
                    }
                ]
            },
            new Track
            {
                Id = "arrays",
                Title = "Arrays",
                Lessons =
                [
                    new Lesson
                    {
                        Id = "js-arr-1",
                        Title = "Array basics",
                        Exercises =
                        [
                            new Exercise
                            {
                                Type = ExerciseType.FillBlank,
                                Prompt = "Add 'potion' to the inventory array",
                                Code = "inventory.[[push]]('potion');",
                                AcceptedAnswers = ["push"],
                                Explanation = "Array.push() appends an item to the end.",
                                Language = "javascript", Track = "arrays", Skill = "push", Difficulty = 1
                            },
                            new Exercise
                            {
                                Type = ExerciseType.PredictOutput,
                                Prompt = "What is logged?",
                                Code = "const nums = [10, 20, 30];\nconsole.log(nums[1]);",
                                ExpectedOutput = "20",
                                Explanation = "Arrays are 0-indexed. Index 1 is the second element, 20.",
                                Language = "javascript", Track = "arrays", Skill = "indexing", Difficulty = 1
                            }
                        ]
                    }
                ]
            }
        ]
    };

    // ── C++ ──────────────────────────────────────────────────────────────────

    private static LanguageCurriculum BuildCpp() => new()
    {
        Id = "cpp",
        DisplayName = "C++",
        Emoji = "⚙️",
        AccentClass = "cpp",
        Tracks =
        [
            new Track
            {
                Id = "basics",
                Title = "Basics",
                Lessons =
                [
                    new Lesson
                    {
                        Id = "cpp-basics-1",
                        Title = "Hello, world!",
                        Exercises =
                        [
                            new Exercise
                            {
                                Type = ExerciseType.FillBlank,
                                Prompt = "Complete the line to print 'Hello, world!'",
                                Code = "[[cout]] << \"Hello, world!\";",
                                AcceptedAnswers = ["cout"],
                                Explanation = "cout is the standard output stream in C++.",
                                Language = "cpp", Track = "basics", Skill = "output", Difficulty = 1
                            },
                            new Exercise
                            {
                                Type = ExerciseType.MultipleChoice,
                                Prompt = "Which header is required to use cout?",
                                Options = ["<stdio.h>", "<iostream>", "<conio.h>", "<string>"],
                                CorrectIndex = 1,
                                Explanation = "#include <iostream> is required for cout and cin.",
                                Language = "cpp", Track = "basics", Skill = "headers", Difficulty = 1
                            },
                            new Exercise
                            {
                                Type = ExerciseType.ArrangeCode,
                                Prompt = "Arrange a complete Hello World program",
                                Code = "#include <iostream>\nusing namespace std;\nint main() {\n    cout << \"Hello!\";\n    return 0;\n}",
                                Language = "cpp", Track = "basics", Skill = "structure", Difficulty = 2
                            }
                        ]
                    }
                ]
            },
            new Track
            {
                Id = "variables",
                Title = "Variables & Types",
                Lessons =
                [
                    new Lesson
                    {
                        Id = "cpp-vars-1",
                        Title = "Typed variables",
                        Exercises =
                        [
                            new Exercise
                            {
                                Type = ExerciseType.FillBlank,
                                Prompt = "Declare an integer variable called health",
                                Code = "[[int]] health = 100;",
                                AcceptedAnswers = ["int"],
                                Explanation = "int is the integer type in C++.",
                                Language = "cpp", Track = "variables", Skill = "types", Difficulty = 1
                            },
                            new Exercise
                            {
                                Type = ExerciseType.MultipleChoice,
                                Prompt = "Which type holds a decimal number?",
                                Options = ["int", "char", "double", "bool"],
                                CorrectIndex = 2,
                                Explanation = "double stores floating-point (decimal) numbers.",
                                Language = "cpp", Track = "variables", Skill = "types", Difficulty = 1
                            }
                        ]
                    }
                ]
            },
            new Track
            {
                Id = "loops",
                Title = "Loops",
                Lessons =
                [
                    new Lesson
                    {
                        Id = "cpp-loops-1",
                        Title = "for loops",
                        Exercises =
                        [
                            new Exercise
                            {
                                Type = ExerciseType.FillBlank,
                                Prompt = "Complete the for loop to count from 0 to 4",
                                Code = "for (int i = 0; i [[<]] 5; i++) {\n    cout << i;\n}",
                                AcceptedAnswers = ["<"],
                                Explanation = "< (less than) is the condition: loop while i is less than 5.",
                                Language = "cpp", Track = "loops", Skill = "for", Difficulty = 1
                            },
                            new Exercise
                            {
                                Type = ExerciseType.PredictOutput,
                                Prompt = "What is the output?",
                                Code = "for (int i = 0; i < 3; i++) {\n    cout << i << \" \";\n}",
                                ExpectedOutput = "0 1 2 ",
                                Explanation = "Prints 0, 1, 2 each followed by a space.",
                                Language = "cpp", Track = "loops", Skill = "for", Difficulty = 2
                            }
                        ]
                    }
                ]
            },
            new Track
            {
                Id = "conditionals",
                Title = "Conditionals",
                Lessons =
                [
                    new Lesson
                    {
                        Id = "cpp-cond-1",
                        Title = "if / else",
                        Exercises =
                        [
                            new Exercise
                            {
                                Type = ExerciseType.FillBlank,
                                Prompt = "Complete the else branch",
                                Code = "if (hp > 0) {\n    cout << \"Alive\";\n} [[else]] {\n    cout << \"Dead\";\n}",
                                AcceptedAnswers = ["else"],
                                Explanation = "else handles the case when the if condition is false.",
                                Language = "cpp", Track = "conditionals", Skill = "if-else", Difficulty = 1
                            }
                        ]
                    }
                ]
            },
            new Track
            {
                Id = "functions",
                Title = "Functions",
                Lessons =
                [
                    new Lesson
                    {
                        Id = "cpp-func-1",
                        Title = "Defining functions",
                        Exercises =
                        [
                            new Exercise
                            {
                                Type = ExerciseType.FillBlank,
                                Prompt = "Complete the return type for a function that returns nothing",
                                Code = "[[void]] greet() {\n    cout << \"Hi!\";\n}",
                                AcceptedAnswers = ["void"],
                                Explanation = "void means the function doesn't return a value.",
                                Language = "cpp", Track = "functions", Skill = "void", Difficulty = 1
                            },
                            new Exercise
                            {
                                Type = ExerciseType.MultipleChoice,
                                Prompt = "What must main() return in a standard C++ program?",
                                Options = ["void", "string", "int", "bool"],
                                CorrectIndex = 2,
                                Explanation = "main() must return int. A return value of 0 signals success.",
                                Language = "cpp", Track = "functions", Skill = "main", Difficulty = 2
                            }
                        ]
                    }
                ]
            }
        ]
    };
}
