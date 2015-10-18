ALTER TABLE [dbo].[StoreVariantGroups]
    ADD CONSTRAINT [DF_StoreVariantGroups_IsDeleted] DEFAULT ((0)) FOR [IsDeleted];

