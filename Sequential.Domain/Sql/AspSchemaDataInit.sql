﻿INSERT INTO [dbo].[aspnet_Applications] ([ApplicationName], [LoweredApplicationName], [ApplicationId], [Description]) VALUES (N'/', N'/', N'f9ecc81c-4362-4572-a04f-b96dc39f6633', NULL)
INSERT INTO [dbo].[aspnet_Users] ([ApplicationId], [UserId], [UserName], [LoweredUserName], [MobileAlias], [IsAnonymous], [LastActivityDate]) VALUES (N'f9ecc81c-4362-4572-a04f-b96dc39f6633', N'c5687881-29aa-4287-9125-1c342288afdb', N'ctsui', N'ctsui', NULL, 0, N'2012-11-02 04:32:52')
INSERT INTO [dbo].[aspnet_Membership] ([ApplicationId], [UserId], [Password], [PasswordFormat], [PasswordSalt], [MobilePIN], [Email], [LoweredEmail], [PasswordQuestion], [PasswordAnswer], [IsApproved], [IsLockedOut], [CreateDate], [LastLoginDate], [LastPasswordChangedDate], [LastLockoutDate], [FailedPasswordAttemptCount], [FailedPasswordAttemptWindowStart], [FailedPasswordAnswerAttemptCount], [FailedPasswordAnswerAttemptWindowStart], [Comment]) VALUES (N'f9ecc81c-4362-4572-a04f-b96dc39f6633', N'c5687881-29aa-4287-9125-1c342288afdb', N'6OT9GB5InCmPwVTntJT1qCixG74=', 1, N'YrdPKfgwa3lH45QhsKwXTg==', NULL, N'ctsui@example.com', N'ctsui@example.com', NULL, NULL, 1, 0, N'2012-10-30 01:59:30', N'2012-11-02 04:32:52', N'2012-10-30 01:59:30', N'1754-01-01 00:00:00', 0, N'1754-01-01 00:00:00', 0, N'1754-01-01 00:00:00', NULL)