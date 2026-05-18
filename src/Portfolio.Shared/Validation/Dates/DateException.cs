namespace Validation.Dates;

public class InvalidDateException(string? msg = null) : Exception(msg) { }

public class MissingDateException(string? msg = null) : Exception(msg){ }