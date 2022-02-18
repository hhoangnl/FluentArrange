// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace FluentArrange.Tests.TestClasses
{
    public class MultipleCtor
    {
        public int CtorUsed { get; }

        public MultipleCtor()
        {
            CtorUsed = 1;
        }

        public MultipleCtor(IFoo foo)
        {
            CtorUsed = 2;
        }
    }
}
