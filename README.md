# NCOM

![Build](https://github.com/harry-harris-27/NCOM/workflows/Build/badge.svg)
[![Coverage Status](https://coveralls.io/repos/github/harry-harris-27/NCOM/badge.svg?branch=master)](https://coveralls.io/github/harry-harris-27/NCOM?branch=master)

A small portable .NET library built for encoding and decoding [OxTS](https://www.oxts.com/) NCOM binary data packets. Note that this library is not currently complete.

NCOM is a data format designed by OxTS for the efficient communication of navigation measurements and other data. It is a very compact format and only includes core measurements, which makes it particularly suitable for inertial navigation systems. For more information about the NCOM data packet, users should read OxTS's NCOM [manual](https://oxts.app.box.com/s/jovnqyj3lkcht2b2159jefzbt950j1n1).

This library is designed to allow developers to create their own custom .NET applications for their specified needs. OxTS already provides a [NCOM library](https://github.com/OxfordTechnicalSolutions/NCOMdecoder), written in C.

## Getting Started
These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. Since this project is not complete and still under development, it should not be used as part of any deployment.

To include this project as part of an existing solution, [download](https://github.com/harry-harris-27/NCOM/master/archive/master.zip) the repository and unzip. Now add the unzipped NCOM and NCOM.Test projects to your solution and add references as required.

Once a initial release of the library has been created, it is planned that a nuget package will be created.

## Code Example
### Decoding
```C#
// Get NCOM data from somewhere. E.g. an UDP packet.
byte[] data;
var buffer = data.AsSpan();

List<NCOMPacket> pkts = new NCOMPacketFactory().ProcessNCOM(buffer);
foreach (NCOMPacket pkt in pkts)
{
    // Check that decoded packet was of type Structure-A.
    if (pkt is NCOMPacketA ncomData)
    {
        // Here we have our decoded NCOM data.
        // ...
    }
}
```
or
```C#
// Decoding a single NCOM packet
NCOMPacketA pkt = new NCOMPacketA();

// Get NCOM data from somewhere...
Span<byte> data;

// Decode
pkt.Unmarshal(data);
```

### Encoding
```C#
NCOMPacketA ncom = new NCOMPacketA();
byte[] encoded = ncom.Marshal();

// Now do want you want with marshalled NCOM data. E.g. send out in UDP packet.
```
or
```C#
NCOMPacketA ncom = new NCOMPacketA();
byte[] buffer = new byte[NCOMPacketA.PacketLength];

ncom.Marshal(buffer.AsSpan());

// Note: This method allows for reuse of buffer. If regular marshalling is require this method will seriously reduce excessive memory allocation
```

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what changes you would like to make.

Please make sure to update tests as appropriate.

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
