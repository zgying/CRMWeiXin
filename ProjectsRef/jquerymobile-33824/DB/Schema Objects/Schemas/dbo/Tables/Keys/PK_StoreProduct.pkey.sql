﻿ALTER TABLE [dbo].[StoreProduct]
    ADD CONSTRAINT [PK_StoreProduct] PRIMARY KEY CLUSTERED ([ProductID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);
