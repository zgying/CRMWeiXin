﻿ALTER TABLE [dbo].[ContentType]
    ADD CONSTRAINT [PK_ContentType] PRIMARY KEY CLUSTERED ([ContentTypeID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

