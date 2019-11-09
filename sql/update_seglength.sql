

update UDS set SEGLENGTH=iif(DATA.STLength() *0.9 < 0.0002694945,  DATA.STLength() * 0.9, iif(DATA.STLength() * 0.5 > 0.0002694945,  DATA.STLength() * 0.5, 0.0002694945));