﻿ALTER TABLE [dbo].[StoreCart]
    ADD CONSTRAINT [PK_StoreCart] PRIMARY KEY CLUSTERED ([CartID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

