﻿using ExtBlock.Core.State;

namespace ExtBlock.Core.State
{
    public interface IStatePropertyProvider
    {
        public StatePropertyList Properties { get; }
    }
}
