# NCOM
An library built in C# for the encoding and decoding the [OxTS](https://www.oxts.com/) NCOM binary data packet. 

Note that this library is not complete:
- [ ] Finish adding Status Channel Enumerations.
- [ ] Finish implementing Status Channels.
- [ ] Test thoroughly. :)

## Code Example
Decoding
```C#
// Get NCOM data from somewhere. E.g. an UDP packet.
byte[] data;

List<NcomPacket> pkts = new NcomPacketFactory().ProcessNcom(data, 0);
foreach (NcomPacket pkt in pkts)
{
    NcomPacketA ncomData = pkt as NcomPacketA;  // Check that decoded packet was of type Structure-A.
    if (ncomData != null)
    {
        // Here we have our decoded NCOM data.
        // ...
    }
}
```

Encoding
```C#
NcomPacketA ncom = new NcomPacketA();

// ...

byte[] encoded = ncom.Marshal();

// Now do want you want with marshalled NCOM data. E.g. send out in UDP packet.
// ...
```
