using System.Text.RegularExpressions;
using Codeigo.Models;

namespace Codeigo.Services;

public class ExerciseResult
{
    public bool IsCorrect { get; set; }
    public string Explanation { get; set; } = "";
    public string? ExpectedAnswer { get; set; }
}

public class ExerciseEngineService
{
    /// <summary>
    /// Checks the user's answer against the exercise definition.
    /// </summary>
    public ExerciseResult CheckAnswer(Exercise exercise, string? userAnswer, int? selectedOptionIndex = null, List<string>? orderedTokens = null)
    {
        var result = new ExerciseResult
        {
            Explanation = exercise.Explanation ?? ""
        };

        switch (exercise.Type)
        {
            case ExerciseType.MultipleChoice:
                if (selectedOptionIndex.HasValue)
                {
                    result.IsCorrect = selectedOptionIndex.Value == exercise.CorrectIndex;
                    if (exercise.Options.Count > exercise.CorrectIndex && exercise.CorrectIndex >= 0)
                    {
                        result.ExpectedAnswer = exercise.Options[exercise.CorrectIndex];
                    }
                }
                else if (!string.IsNullOrWhiteSpace(userAnswer) && exercise.Options.Count > exercise.CorrectIndex)
                {
                    result.IsCorrect = Normalize(userAnswer) == Normalize(exercise.Options[exercise.CorrectIndex]);
                    result.ExpectedAnswer = exercise.Options[exercise.CorrectIndex];
                }
                break;

            case ExerciseType.FillBlank:
                var trimmedUser = Normalize(userAnswer ?? "");
                var acceptedBlank = exercise.AcceptedAnswers.Select(Normalize).ToList();

                // If no explicit accepted answers, extract from [[...]]
                if (acceptedBlank.Count == 0 && !string.IsNullOrEmpty(exercise.Code))
                {
                    var match = Regex.Match(exercise.Code, @"\[\[(.*?)\]\]");
                    if (match.Success)
                    {
                        acceptedBlank.Add(Normalize(match.Groups[1].Value));
                    }
                }

                result.IsCorrect = acceptedBlank.Any(a => a == trimmedUser);
                result.ExpectedAnswer = acceptedBlank.FirstOrDefault() ?? "";
                break;

            case ExerciseType.PredictOutput:
                var normExpected = NormalizeCodeOutput(exercise.ExpectedOutput ?? "");
                var normActual = NormalizeCodeOutput(userAnswer ?? "");
                result.IsCorrect = normExpected == normActual;
                result.ExpectedAnswer = exercise.ExpectedOutput ?? "";
                break;

            case ExerciseType.ArrangeCode:
                if (orderedTokens != null && orderedTokens.Count > 0)
                {
                    var combinedUser = string.Join(" ", orderedTokens.Select(t => t.Trim())).Trim();
                    // Extract clean tokens from target code
                    var cleanCode = Regex.Replace(exercise.Code ?? "", @"\s+", " ").Trim();
                    result.IsCorrect = Normalize(combinedUser) == Normalize(cleanCode);
                    result.ExpectedAnswer = exercise.Code ?? "";
                }
                else if (!string.IsNullOrWhiteSpace(userAnswer))
                {
                    result.IsCorrect = Normalize(userAnswer) == Normalize(exercise.Code ?? "");
                    result.ExpectedAnswer = exercise.Code ?? "";
                }
                break;

            case ExerciseType.TypeItOut:
                var typed = Normalize(userAnswer ?? "");
                var acceptedTyped = exercise.AcceptedAnswers.Select(Normalize).ToList();
                if (!string.IsNullOrEmpty(exercise.TargetLine))
                {
                    acceptedTyped.Add(Normalize(exercise.TargetLine));
                }

                result.IsCorrect = acceptedTyped.Any(a => a == typed);
                result.ExpectedAnswer = exercise.TargetLine ?? acceptedTyped.FirstOrDefault() ?? "";
                break;
        }

        return result;
    }

    private static string Normalize(string input)
    {
        if (string.IsNullOrEmpty(input)) return "";
        return input.Trim().Replace("\r\n", "\n").Replace("\r", "\n");
    }

    private static string NormalizeCodeOutput(string output)
    {
        if (string.IsNullOrEmpty(output)) return "";
        var lines = output.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n');
        return string.Join("\n", lines.Select(l => l.TrimEnd())).Trim();
    }
}
