ALTER TABLE [dbo].[TaxRate]
    ADD CONSTRAINT [DF_TaxRate_CreateDate] DEFAULT (getdate()) FOR [CreateDate];

