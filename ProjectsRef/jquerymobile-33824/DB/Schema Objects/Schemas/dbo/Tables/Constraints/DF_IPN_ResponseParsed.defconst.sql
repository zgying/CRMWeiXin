ALTER TABLE [dbo].[IPN]
    ADD CONSTRAINT [DF_IPN_ResponseParsed] DEFAULT ((0)) FOR [ResponseParsed];

