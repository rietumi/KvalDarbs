using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicCore.Util
{
    public class LocalizedErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError ConcurrencyFailure() => new IdentityError { Code = nameof(ConcurrencyFailure), Description = ErrorText.ConcurrencyFailure };
        public override IdentityError DefaultError() => new IdentityError { Code = nameof(DefaultError), Description = ErrorText.DefaultError };
        public override IdentityError DuplicateEmail(string email) => new IdentityError { Code = nameof(DuplicateEmail), Description = string.Format(ErrorText.DuplicateEmail, email) };
        public override IdentityError DuplicateRoleName(string role) => new IdentityError { Code = nameof(DuplicateRoleName), Description = string.Format(ErrorText.DuplicateRoleName, role) };
        public override IdentityError DuplicateUserName(string userName) => new IdentityError { Code = nameof(DuplicateUserName), Description = string.Format(ErrorText.DuplicateUserName, userName) };
        public override IdentityError InvalidEmail(string email) => new IdentityError { Code = nameof(InvalidEmail), Description = string.Format(ErrorText.InvalidEmail, email) };
        public override IdentityError InvalidRoleName(string role) => new IdentityError { Code = nameof(InvalidRoleName), Description = string.Format(ErrorText.InvalidRoleName, role) };
        public override IdentityError InvalidToken() => new IdentityError { Code = nameof(InvalidToken), Description = ErrorText.InvalidToken };
        public override IdentityError InvalidUserName(string userName) => new IdentityError { Code = nameof(InvalidUserName), Description = string.Format(ErrorText.InvalidUserName, userName) };
        public override IdentityError LoginAlreadyAssociated() => new IdentityError { Code = nameof(LoginAlreadyAssociated), Description = ErrorText.LoginAlreadyAssociated };
        public override IdentityError PasswordMismatch() => new IdentityError { Code = nameof(PasswordMismatch), Description = ErrorText.PasswordMismatch };
        public override IdentityError PasswordRequiresDigit() => new IdentityError { Code = nameof(PasswordRequiresDigit), Description = ErrorText.PasswordRequiresDigit };
        public override IdentityError PasswordRequiresLower() => new IdentityError { Code = nameof(PasswordRequiresLower), Description = ErrorText.PasswordRequiresLower };
        public override IdentityError PasswordRequiresNonAlphanumeric() => new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = ErrorText.PasswordRequiresNonAlphanumeric };
        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars) => new IdentityError { Code = nameof(PasswordRequiresUniqueChars), Description = string.Format(ErrorText.PasswordRequiresUniqueChars, uniqueChars) };
        public override IdentityError PasswordRequiresUpper() => new IdentityError { Code = nameof(PasswordRequiresUpper), Description = ErrorText.PasswordRequiresUpper };
        public override IdentityError PasswordTooShort(int length) => new IdentityError { Code = nameof(PasswordTooShort), Description = string.Format(ErrorText.PasswordTooShort, length) };
        public override IdentityError RecoveryCodeRedemptionFailed() => new IdentityError { Code = nameof(RecoveryCodeRedemptionFailed), Description = ErrorText.RecoveryCodeRedemptionFailed };
        public override IdentityError UserAlreadyHasPassword() => new IdentityError { Code = nameof(UserAlreadyHasPassword), Description = ErrorText.UserAlreadyHasPassword };
        public override IdentityError UserAlreadyInRole(string role) => new IdentityError { Code = nameof(UserAlreadyInRole), Description = string.Format(ErrorText.UserAlreadyInRole, role) };
        public override IdentityError UserLockoutNotEnabled() => new IdentityError { Code = nameof(UserLockoutNotEnabled), Description = ErrorText.UserLockoutNotEnabled };
        public override IdentityError UserNotInRole(string role) => new IdentityError { Code = nameof(UserNotInRole), Description = string.Format(ErrorText.UserNotInRole, role) };
    }
}
