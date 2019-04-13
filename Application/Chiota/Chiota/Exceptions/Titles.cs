﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Chiota.Exceptions
{
    public static class Titles
    {
        #region Base exceptions

        public static string Unknown = "Unknown";
        public static string InvalidUserInput = "Invalid user input";
        public static string MissingUserInput = "Missing user input";
        public static string FailedLoadingFile = "Loading file failed";

        #endregion

        #region Base exceptions

        public static string AuthFailedPasswordConfirmation = "Failed password confirmation";
        public static string AuthMissingSeed = "Missing seed";

        #endregion

        #region Back up exceptions

        public const string BackUpFailedSeedConfirmation = "Failed seed confirmation";

        #endregion
    }
}
