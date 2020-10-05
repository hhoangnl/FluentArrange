// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace FluentArrange.Tests.TestClasses
{
    public class AccountService
    {
        public IAccountRepository AccountRepository { get; }

        public AccountService(IAccountRepository accountRepository)
        {
            AccountRepository = accountRepository;
        }
    }
}
