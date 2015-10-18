ALTER TABLE [dbo].[StoreVariantGroups]
    ADD CONSTRAINT [DF_StoreVariantGroups_CreateDate] DEFAULT (getdate()) FOR [CreateDate];

