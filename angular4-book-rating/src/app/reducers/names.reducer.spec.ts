import { reducer } from './names.reducer';
import * as SI from 'seamless-immutable';

describe('a names Reducer', () => {
  it('should count names', () => {
    const names = [
      'Erich',
      'Richard',
      'Ralph',
      'John',
      'Richard'
    ];

    // debugger;
    const accumulator = SI.from({});

    const result = names.reduce(reducer, accumulator);

    expect(result).toEqual({
      Erich: 1,
      Richard: 2,
      Ralph: 1,
      John: 1
    });
  });
});
