ALTER TABLE [dbo].[StoreVariants]
    ADD CONSTRAINT [DF_StoreVariants_IsDeleted] DEFAULT ((0)) FOR [IsDeleted];

