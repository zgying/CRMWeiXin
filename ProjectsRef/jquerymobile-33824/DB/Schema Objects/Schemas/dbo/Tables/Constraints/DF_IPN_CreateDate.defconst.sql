ALTER TABLE [dbo].[IPN]
    ADD CONSTRAINT [DF_IPN_CreateDate] DEFAULT (getdate()) FOR [CreateDate];

