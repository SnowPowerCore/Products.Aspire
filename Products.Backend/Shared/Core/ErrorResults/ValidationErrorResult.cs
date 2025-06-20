﻿using FluentValidation.Results;
using MaybeResults;

namespace Products.Backend.Core.ErrorResults;

[None]
public sealed partial record ValidationErrorResult<T>
{
    // Add a custom constructor to create a ValidationErrorResult<T> from a FluentValidation ValidationResult
    public ValidationErrorResult(ValidationResult validationResult)
    {
        // A valid validationResult means it's a dev error that needs to be fixed immediately
        // for that reason it's ok to throw an exception here.
        if (validationResult.IsValid)
            throw new ValidationErrorResultException();

        Message = typeof(T).Name;

        var errorList = new List<NoneDetail>(validationResult.Errors.Count);
        errorList.AddRange(validationResult.Errors.Select(e => new NoneDetail(e.PropertyName, e.ErrorMessage)));
        Details = errorList;
    }
}

public sealed class ValidationErrorResultException : Exception
{
    public ValidationErrorResultException() : base($"{nameof(ValidationErrorResult)} cannot be created from a successful validation") { }
}