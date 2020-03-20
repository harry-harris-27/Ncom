# Ncom

![Build](https://github.com/harry-harris-27/Ncom/workflows/Build/badge.svg)
[![Coverage Status](https://coveralls.io/repos/github/harry-harris-27/Ncom/badge.svg?branch=master)](https://coveralls.io/github/harry-harris-27/Ncom?branch=master)

A small portable .NET library built for encoding and decoding [OxTS](https://www.oxts.com/) NCOM binary data packets. Note that this library is not currently complete.

NCOM is a data format designed by OxTS for the efficient communication of navigation measurements and other data. It is a very compact format and only includes core measurements, which makes it particularly suitable for inertial navigation systems. For more information about the NCOM data packet, users should read OxTS's NCOM [manual](https://oxts.app.box.com/s/jovnqyj3lkcht2b2159jefzbt950j1n1).

This library is designed to allow developers to create their own custom .NET applications for their specified needs. OxTS already provides a [NCOM library](https://github.com/OxfordTechnicalSolutions/NCOMdecoder), written in C.

## Getting Started
These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. Since this project is not complete and still under development, it should not be used as part of any deployment.

To include this project as part of an existing solution, [download](https://github.com/harry-harris-27/Ncom/master/archive/master.zip) the repository and unzip. Now add the unzipped Ncom and Ncom.Test projects to your solution and add references as required.

Once a initial release of the library has been created, it is planned that a nuget package will be created.

## Code Example
### Decoding
```C#
// Get Ncom data from somewhere. E.g. an UDP packet.
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
or
```C#
// Decoding a single NCOM packet
NcomPacketA pkt = new NcomPacketA();

// Get NCOM data from somewhere
byte[] data;

// Decode
pkt.Unmarshal(data, 0);
```

### Encoding
```C#
NcomPacketA ncom = new NcomPacketA();

// ...

byte[] encoded = ncom.Marshal();

// Now do want you want with marshalled NCOM data. E.g. send out in UDP packet.
// ...
```

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what changes you would like to make.

Please make sure to update tests as appropriate.

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
