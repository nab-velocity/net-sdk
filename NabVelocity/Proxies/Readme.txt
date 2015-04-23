To convert from 2.0 to 2.0.18 you must change these things:

(1) CWSBankcardClient to CwsTransactionProcessingClient
(2) AVSData TypeStateProvince changed to a string
(3) Remove MerchantProfileId from Capture/ReturnById/Undo/CaptureSelective operation params
